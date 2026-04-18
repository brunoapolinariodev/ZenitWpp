using ZenitWpp.Domain.Common;

namespace ZenitWpp.Domain.Conversations.Events;

public class MessageSentEvent : BaseDomainEvent
{
    public Guid ConversationId { get; }
    public Guid MessageId { get; }
    public string Content { get; }
    public MessageDirection Direction { get; }

    public MessageSentEvent(Guid conversationId, Guid messageId, string content, MessageDirection direction)
    {
        ConversationId = conversationId;
        MessageId = messageId;
        Content = content;
        Direction = direction;
    }
}
