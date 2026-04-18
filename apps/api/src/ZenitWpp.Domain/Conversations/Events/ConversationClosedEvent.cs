using ZenitWpp.Domain.Common;

namespace ZenitWpp.Domain.Conversations.Events;

public class ConversationClosedEvent : BaseDomainEvent
{
    public Guid ConversationId { get; }
    public Guid ContactId { get; }

    public ConversationClosedEvent(Guid conversationId, Guid contactId)
    {
        ConversationId = conversationId;
        ContactId = contactId;
    }
}
