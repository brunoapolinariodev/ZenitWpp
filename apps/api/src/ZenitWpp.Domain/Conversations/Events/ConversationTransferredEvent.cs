using ZenitWpp.Domain.Common;

namespace ZenitWpp.Domain.Conversations.Events;

public class ConversationTransferredEvent : BaseDomainEvent
{
    public Guid ConversationId { get; }
    public Guid? PreviousAgentId { get; }
    public Guid NewAgentId { get; }

    public ConversationTransferredEvent(Guid conversationId, Guid? previousAgentId, Guid newAgentId)
    {
        ConversationId = conversationId;
        PreviousAgentId = previousAgentId;
        NewAgentId = newAgentId;
    }
}
