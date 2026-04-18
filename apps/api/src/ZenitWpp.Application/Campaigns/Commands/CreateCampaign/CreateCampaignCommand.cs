using MediatR;
using ZenitWpp.Application.Campaigns.DTOs;

namespace ZenitWpp.Application.Campaigns.Commands.CreateCampaign;

public record CreateCampaignCommand(
    string Name,
    string Message,
    string? Segment,
    DateTime? ScheduledAt
) : IRequest<CampaignResponse>;
