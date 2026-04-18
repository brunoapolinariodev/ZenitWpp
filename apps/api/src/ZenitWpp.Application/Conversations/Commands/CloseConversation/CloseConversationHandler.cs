using MediatR;
using ZenitWpp.Domain.Conversations.Repositories;

namespace ZenitWpp.Application.Conversations.Commands.CloseConversation;

public class CloseConversationHandler : IRequestHandler<CloseConversationCommand>
{
    private readonly IConversationRepository _repository;

    public CloseConversationHandler(IConversationRepository repository)
        => _repository = repository;

    public async Task Handle(CloseConversationCommand cmd, CancellationToken ct)
    {
        var conversation = await _repository.GetByIdAsync(cmd.ConversationId, ct)
            ?? throw new KeyNotFoundException($"Conversa {cmd.ConversationId} não encontrada.");

        conversation.Close();
        await _repository.UpdateAsync(conversation, ct);
    }
}
