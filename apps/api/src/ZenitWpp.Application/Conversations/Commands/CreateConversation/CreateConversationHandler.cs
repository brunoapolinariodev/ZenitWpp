using Mapster;
using MediatR;
using ZenitWpp.Application.Conversations.DTOs;
using ZenitWpp.Domain.Conversations;
using ZenitWpp.Domain.Conversations.Repositories;

namespace ZenitWpp.Application.Conversations.Commands.CreateConversation;

public class CreateConversationHandler : IRequestHandler<CreateConversationCommand, ConversationResponse>
{
    private readonly IConversationRepository _repository;

    public CreateConversationHandler(IConversationRepository repository)
        => _repository = repository;

    public async Task<ConversationResponse> Handle(CreateConversationCommand cmd, CancellationToken ct)
    {
        var conversation = Conversation.Create(cmd.ContactId, cmd.Channel);
        await _repository.AddAsync(conversation, ct);
        return conversation.Adapt<ConversationResponse>();
    }
}
