using ZenitWpp.Domain.Common;

namespace ZenitWpp.Domain.Conversations.Events;

public class ConversationStartedEvent : BaseDomainEvent
{
    public Guid ConversationId { get; }
    public Guid ContactId { get; }

    public ConversationStartedEvent(Guid conversationId, Guid contactId)
    {
        ConversationId = conversationId;
        ContactId = contactId;
    }
}
