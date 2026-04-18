using ZenitWpp.Domain.Conversations;

namespace ZenitWpp.Application.Conversations.DTOs;

public record ConversationResponse(
    Guid Id,
    Guid ContactId,
    Guid? AssignedAgentId,
    ConversationStatus Status,
    string Channel,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    IEnumerable<MessageResponse> Messages
);
