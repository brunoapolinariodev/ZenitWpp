using ZenitWpp.Domain.Campaigns;

namespace ZenitWpp.Application.Campaigns.DTOs;

public record CampaignResponse(
    Guid Id,
    string Name,
    string Message,
    CampaignStatus Status,
    string? Segment,
    int TotalRecipients,
    int SentCount,
    int FailedCount,
    DateTime? ScheduledAt,
    DateTime? SentAt,
    DateTime CreatedAt
);
