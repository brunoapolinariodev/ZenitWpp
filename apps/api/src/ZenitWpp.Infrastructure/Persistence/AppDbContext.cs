using Microsoft.EntityFrameworkCore;
using ZenitWpp.Domain.Agents;
using ZenitWpp.Domain.Automation;
using ZenitWpp.Domain.Campaigns;
using ZenitWpp.Domain.Contacts;
using ZenitWpp.Domain.Conversations;
using ZenitWpp.Domain.Notifications;

namespace ZenitWpp.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Conversation> Conversations => Set<Conversation>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<Agent> Agents => Set<Agent>();
    public DbSet<Flow> Flows => Set<Flow>();
    public DbSet<FlowStep> FlowSteps => Set<FlowStep>();
    public DbSet<Campaign> Campaigns => Set<Campaign>();
    public DbSet<CampaignRecipient> CampaignRecipients => Set<CampaignRecipient>();
    public DbSet<Reminder> Reminders => Set<Reminder>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
