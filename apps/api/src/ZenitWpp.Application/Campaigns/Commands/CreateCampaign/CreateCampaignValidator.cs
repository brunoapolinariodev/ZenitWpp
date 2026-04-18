using FluentValidation;

namespace ZenitWpp.Application.Campaigns.Commands.CreateCampaign;

public class CreateCampaignValidator : AbstractValidator<CreateCampaignCommand>
{
    public CreateCampaignValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Message).NotEmpty().MaximumLength(4096);
        RuleFor(x => x.ScheduledAt).GreaterThan(DateTime.UtcNow).When(x => x.ScheduledAt.HasValue);
    }
}
