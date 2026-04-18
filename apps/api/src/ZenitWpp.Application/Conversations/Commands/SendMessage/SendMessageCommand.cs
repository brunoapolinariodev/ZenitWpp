using MediatR;
using ZenitWpp.Application.Conversations.DTOs;
using ZenitWpp.Domain.Conversations;

namespace ZenitWpp.Application.Conversations.Commands.SendMessage;

public record SendMessageCommand(
    Guid ConversationId,
    string Content,
    MessageType Type,
    MessageDirection Direction
) : IRequest<MessageResponse>;
