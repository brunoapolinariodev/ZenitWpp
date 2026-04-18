using ZenitWpp.Domain.Common;

namespace ZenitWpp.Domain.Campaigns;

public class CampaignRecipient : BaseEntity
{
    public Guid CampaignId { get; private set; }
    public Guid ContactId { get; private set; }
    public RecipientStatus Status { get; private set; }
    public DateTime? SentAt { get; private set; }
    public DateTime? ReadAt { get; private set; }

    private CampaignRecipient() { }

    internal CampaignRecipient(Guid campaignId, Guid contactId)
    {
        CampaignId = campaignId;
        ContactId = contactId;
        Status = RecipientStatus.Pending;
    }

    public void MarkAsSent() { Status = RecipientStatus.Sent; SentAt = DateTime.UtcNow; }
    public void MarkAsRead() { Status = RecipientStatus.Read; ReadAt = DateTime.UtcNow; }
    public void MarkAsFailed() { Status = RecipientStatus.Failed; }
}
