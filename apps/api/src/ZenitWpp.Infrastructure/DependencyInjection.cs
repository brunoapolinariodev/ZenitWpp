using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;
using ZenitWpp.Domain.Agents.Repositories;
using ZenitWpp.Domain.Automation.Repositories;
using ZenitWpp.Domain.Campaigns.Repositories;
using ZenitWpp.Domain.Contacts.Repositories;
using ZenitWpp.Domain.Conversations.Repositories;
using ZenitWpp.Domain.Notifications.Repositories;
using ZenitWpp.Application.Common.Interfaces;
using ZenitWpp.Infrastructure.Auth;
using ZenitWpp.Infrastructure.Cache;
using ZenitWpp.Infrastructure.Integrations.AI;
using ZenitWpp.Infrastructure.Integrations.Storage;
using ZenitWpp.Infrastructure.Integrations.Translation;
using ZenitWpp.Infrastructure.Integrations.WhatsApp;
using ZenitWpp.Infrastructure.Persistence;
using ZenitWpp.Infrastructure.Persistence.Repositories;

namespace ZenitWpp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddPersistence(config);
        services.AddCache(config);
        services.AddAuth(config);
        services.AddHangfireJobs(config);
        services.AddIntegrations(config);

        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("Postgres")));

        services.AddScoped<IConversationRepository, ConversationRepository>();
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<IAgentRepository, AgentRepository>();
        services.AddScoped<IFlowRepository, FlowRepository>();
        services.AddScoped<ICampaignRepository, CampaignRepository>();
        services.AddScoped<IReminderRepository, ReminderRepository>();

        return services;
    }

    private static IServiceCollection AddCache(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(config.GetConnectionString("Redis")!));

        services.AddScoped<ICacheService, RedisCacheService>();

        return services;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<ITotpService, TotpService>();
        services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidAudience = config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(config["Jwt:Secret"]!))
                };
            });

        services.AddAuthorization();

        return services;
    }

    private static IServiceCollection AddHangfireJobs(this IServiceCollection services, IConfiguration config)
    {
        services.AddHangfire(h => h
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(c =>
                c.UseNpgsqlConnection(config.GetConnectionString("Postgres"))));

        services.AddHangfireServer();

        return services;
    }

    private static IServiceCollection AddIntegrations(this IServiceCollection services, IConfiguration config)
    {
        services.AddHttpClient<IWhatsAppService, EvolutionApiService>(client =>
        {
            client.BaseAddress = new Uri(config["EvolutionApi:BaseUrl"]!);
            client.DefaultRequestHeaders.Add("apikey", config["EvolutionApi:ApiKey"]);
        });

        services.AddHttpClient<IAIService, ClaudeAIService>(client =>
        {
            client.BaseAddress = new Uri("https://api.anthropic.com");
            client.DefaultRequestHeaders.Add("x-api-key", config["Claude:ApiKey"]);
            client.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
        });

        services.AddScoped<IStorageService, NullStorageService>();
        services.AddScoped<ITranslationService, NullTranslationService>();

        return services;
    }
}
