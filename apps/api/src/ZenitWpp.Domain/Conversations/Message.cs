using ZenitWpp.Domain.Common;

namespace ZenitWpp.Domain.Conversations;

public class Message : BaseEntity
{
    public Guid ConversationId { get; private set; }
    public string Content { get; private set; } = string.Empty;
    public MessageType Type { get; private set; }
    public MessageDirection Direction { get; private set; }
    public DateTime SentAt { get; private set; }

    private Message() { }

    internal Message(Guid conversationId, string content, MessageType type, MessageDirection direction)
    {
        ConversationId = conversationId;
        Content = content;
        Type = type;
        Direction = direction;
        SentAt = DateTime.UtcNow;
    }
}
