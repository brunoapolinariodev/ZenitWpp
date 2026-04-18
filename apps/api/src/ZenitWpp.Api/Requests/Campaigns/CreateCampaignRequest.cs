namespace ZenitWpp.Api.Requests.Campaigns;

public record CreateCampaignRequest(string Name, string Message, string? Segment, DateTime? ScheduledAt);
