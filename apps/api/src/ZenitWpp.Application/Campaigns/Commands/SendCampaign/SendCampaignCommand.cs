using MediatR;

namespace ZenitWpp.Application.Campaigns.Commands.SendCampaign;

public record SendCampaignCommand(Guid CampaignId) : IRequest;
