using Mapster;
using MediatR;
using ZenitWpp.Application.Conversations.DTOs;
using ZenitWpp.Domain.Conversations.Repositories;

namespace ZenitWpp.Application.Conversations.Commands.SendMessage;

public class SendMessageHandler : IRequestHandler<SendMessageCommand, MessageResponse>
{
    private readonly IConversationRepository _repository;

    public SendMessageHandler(IConversationRepository repository)
        => _repository = repository;

    public async Task<MessageResponse> Handle(SendMessageCommand cmd, CancellationToken ct)
    {
        var conversation = await _repository.GetByIdAsync(cmd.ConversationId, ct)
            ?? throw new KeyNotFoundException($"Conversa {cmd.ConversationId} não encontrada.");

        conversation.SendMessage(cmd.Content, cmd.Type, cmd.Direction);
        await _repository.UpdateAsync(conversation, ct);

        var message = conversation.Messages.Last();
        return message.Adapt<MessageResponse>();
    }
}
