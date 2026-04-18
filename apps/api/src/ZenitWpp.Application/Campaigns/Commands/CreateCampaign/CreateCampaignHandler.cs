using Mapster;
using MediatR;
using ZenitWpp.Application.Campaigns.DTOs;
using ZenitWpp.Domain.Campaigns;
using ZenitWpp.Domain.Campaigns.Repositories;
using ZenitWpp.Domain.Contacts.Repositories;

namespace ZenitWpp.Application.Campaigns.Commands.CreateCampaign;

public class CreateCampaignHandler : IRequestHandler<CreateCampaignCommand, CampaignResponse>
{
    private readonly ICampaignRepository _campaigns;
    private readonly IContactRepository _contacts;

    public CreateCampaignHandler(ICampaignRepository campaigns, IContactRepository contacts)
    {
        _campaigns = campaigns;
        _contacts = contacts;
    }

    public async Task<CampaignResponse> Handle(CreateCampaignCommand cmd, CancellationToken ct)
    {
        var campaign = Campaign.Create(cmd.Name, cmd.Message, cmd.Segment, cmd.ScheduledAt);

        var contacts = await _contacts.ListAsync(1, int.MaxValue, ct);
        var filtered = cmd.Segment is null
            ? contacts
            : contacts.Where(c => c.Segment == cmd.Segment);

        foreach (var contact in filtered)
            campaign.AddRecipient(contact.Id);

        await _campaigns.AddAsync(campaign, ct);
        return campaign.Adapt<CampaignResponse>();
    }
}
