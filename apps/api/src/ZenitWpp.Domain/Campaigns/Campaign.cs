using ZenitWpp.Domain.Common;
using ZenitWpp.Domain.Campaigns.Events;

namespace ZenitWpp.Domain.Campaigns;

public class Campaign : AggregateRoot
{
    private readonly List<CampaignRecipient> _recipients = new();

    public string Name { get; private set; } = string.Empty;
    public string Message { get; private set; } = string.Empty;
    public CampaignStatus Status { get; private set; }
    public string? Segment { get; private set; }
    public DateTime? ScheduledAt { get; private set; }
    public DateTime? SentAt { get; private set; }
    public IReadOnlyCollection<CampaignRecipient> Recipients => _recipients.AsReadOnly();

    private Campaign() { }

    public static Campaign Create(string name, string message, string? segment = null, DateTime? scheduledAt = null)
    {
        return new Campaign
        {
            Name = name,
            Message = message,
            Status = CampaignStatus.Draft,
            Segment = segment,
            ScheduledAt = scheduledAt
        };
    }

    public void AddRecipient(Guid contactId)
    {
        _recipients.Add(new CampaignRecipient(Id, contactId));
        SetUpdatedAt();
    }

    public void Start()
    {
        Status = CampaignStatus.Running;
        SentAt = DateTime.UtcNow;
        SetUpdatedAt();
    }

    public void Complete()
    {
        Status = CampaignStatus.Completed;
        SetUpdatedAt();
        AddDomainEvent(new CampaignSentEvent(Id, Name, _recipients.Count));
    }

    public void Cancel()
    {
        Status = CampaignStatus.Cancelled;
        SetUpdatedAt();
    }
}
