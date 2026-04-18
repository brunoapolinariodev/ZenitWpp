using ZenitWpp.Domain.Conversations;

namespace ZenitWpp.Application.Conversations.DTOs;

public record MessageResponse(
    Guid Id,
    Guid ConversationId,
    string Content,
    MessageType Type,
    MessageDirection Direction,
    DateTime SentAt
);
