using FluentValidation;

namespace ZenitWpp.Application.Conversations.Commands.CreateConversation;

public class CreateConversationValidator : AbstractValidator<CreateConversationCommand>
{
    public CreateConversationValidator()
    {
        RuleFor(x => x.ContactId).NotEmpty();
        RuleFor(x => x.Channel).NotEmpty().MaximumLength(50);
    }
}
