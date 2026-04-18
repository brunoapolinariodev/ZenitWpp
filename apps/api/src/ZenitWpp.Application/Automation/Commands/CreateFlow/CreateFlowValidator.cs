using FluentValidation;

namespace ZenitWpp.Application.Automation.Commands.CreateFlow;

public class CreateFlowValidator : AbstractValidator<CreateFlowCommand>
{
    public CreateFlowValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}
