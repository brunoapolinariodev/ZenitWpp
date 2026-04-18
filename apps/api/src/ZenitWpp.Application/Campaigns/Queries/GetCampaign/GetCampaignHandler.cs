using Mapster;
using MediatR;
using ZenitWpp.Application.Campaigns.DTOs;
using ZenitWpp.Domain.Campaigns.Repositories;

namespace ZenitWpp.Application.Campaigns.Queries.GetCampaign;

public class GetCampaignHandler : IRequestHandler<GetCampaignQuery, CampaignResponse>
{
    private readonly ICampaignRepository _repository;

    public GetCampaignHandler(ICampaignRepository repository)
        => _repository = repository;

    public async Task<CampaignResponse> Handle(GetCampaignQuery query, CancellationToken ct)
    {
        var campaign = await _repository.GetByIdAsync(query.Id, ct)
            ?? throw new KeyNotFoundException($"Campanha {query.Id} não encontrada.");

        return campaign.Adapt<CampaignResponse>();
    }
}
