using MediatR;
using ZenitWpp.Application.Common.Interfaces;
using ZenitWpp.Domain.Campaigns.Repositories;
using ZenitWpp.Domain.Contacts.Repositories;

namespace ZenitWpp.Application.Campaigns.Commands.SendCampaign;

public class SendCampaignHandler : IRequestHandler<SendCampaignCommand>
{
    private readonly ICampaignRepository _campaigns;
    private readonly IContactRepository _contacts;
    private readonly IWhatsAppService _whatsApp;

    public SendCampaignHandler(ICampaignRepository campaigns, IContactRepository contacts, IWhatsAppService whatsApp)
    {
        _campaigns = campaigns;
        _contacts = contacts;
        _whatsApp = whatsApp;
    }

    public async Task Handle(SendCampaignCommand cmd, CancellationToken ct)
    {
        var campaign = await _campaigns.GetByIdAsync(cmd.CampaignId, ct)
            ?? throw new KeyNotFoundException($"Campanha {cmd.CampaignId} não encontrada.");

        campaign.Start();

        foreach (var recipient in campaign.Recipients)
        {
            var contact = await _contacts.GetByIdAsync(recipient.ContactId, ct);
            if (contact is null) continue;

            try
            {
                await _whatsApp.SendTextAsync(contact.PhoneNumber.Value, campaign.Message, ct);
                recipient.MarkAsSent();
            }
            catch
            {
                recipient.MarkAsFailed();
            }
        }

        campaign.Complete();
        await _campaigns.UpdateAsync(campaign, ct);
    }
}
