using Mapster;
using MediatR;
using ZenitWpp.Application.Conversations.DTOs;
using ZenitWpp.Domain.Conversations.Repositories;

namespace ZenitWpp.Application.Conversations.Commands.TransferConversation;

public class TransferConversationHandler : IRequestHandler<TransferConversationCommand, ConversationResponse>
{
    private readonly IConversationRepository _repository;

    public TransferConversationHandler(IConversationRepository repository)
        => _repository = repository;

    public async Task<ConversationResponse> Handle(TransferConversationCommand cmd, CancellationToken ct)
    {
        var conversation = await _repository.GetByIdAsync(cmd.ConversationId, ct)
            ?? throw new KeyNotFoundException($"Conversa {cmd.ConversationId} não encontrada.");

        conversation.Transfer(cmd.NewAgentId);
        await _repository.UpdateAsync(conversation, ct);
        return conversation.Adapt<ConversationResponse>();
    }
}
