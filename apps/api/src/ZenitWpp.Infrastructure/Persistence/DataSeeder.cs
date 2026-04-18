using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ZenitWpp.Application.Common.Interfaces;
using ZenitWpp.Domain.Agents;

namespace ZenitWpp.Infrastructure.Persistence;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var db     = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();

        try
        {
            await db.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            logger.LogWarning("Migrate falhou (banco pode não estar disponível): {Message}", ex.Message);
            return;
        }

        const string adminEmail = "admin@zenitwpp.com";

        var exists = await db.Agents.AnyAsync(a => a.Email == adminEmail);
        if (exists)
        {
            logger.LogInformation("Seed: usuário admin já existe, nada a fazer.");
            return;
        }

        var admin = Agent.Create(
            name:         "Administrador",
            email:        adminEmail,
            passwordHash: hasher.Hash("Admin@123"),
            role:         AgentRole.Admin
        );

        db.Agents.Add(admin);
        await db.SaveChangesAsync();

        logger.LogInformation("Seed: usuário admin criado — {Email}", adminEmail);
    }
}
