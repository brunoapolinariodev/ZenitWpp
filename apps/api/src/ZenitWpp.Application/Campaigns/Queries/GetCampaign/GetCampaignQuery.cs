using MediatR;
using ZenitWpp.Application.Campaigns.DTOs;

namespace ZenitWpp.Application.Campaigns.Queries.GetCampaign;

public record GetCampaignQuery(Guid Id) : IRequest<CampaignResponse>;
