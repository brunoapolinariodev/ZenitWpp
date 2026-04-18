using ZenitWpp.Domain.Common;

namespace ZenitWpp.Domain.Campaigns.Events;

public class CampaignSentEvent : BaseDomainEvent
{
    public Guid CampaignId { get; }
    public string CampaignName { get; }
    public int RecipientCount { get; }

    public CampaignSentEvent(Guid campaignId, string campaignName, int recipientCount)
    {
        CampaignId = campaignId;
        CampaignName = campaignName;
        RecipientCount = recipientCount;
    }
}
