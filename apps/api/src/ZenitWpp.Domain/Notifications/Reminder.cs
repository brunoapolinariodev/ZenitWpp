using ZenitWpp.Domain.Common;

namespace ZenitWpp.Domain.Notifications;

public class Reminder : AggregateRoot
{
    public Guid ContactId { get; private set; }
    public Guid? ConversationId { get; private set; }
    public string Message { get; private set; } = string.Empty;
    public DateTime ScheduledAt { get; private set; }
    public bool Sent { get; private set; }
    public DateTime? SentAt { get; private set; }

    private Reminder() { }

    public static Reminder Create(Guid contactId, string message, DateTime scheduledAt, Guid? conversationId = null)
    {
        return new Reminder
        {
            ContactId = contactId,
            ConversationId = conversationId,
            Message = message,
            ScheduledAt = scheduledAt,
            Sent = false
        };
    }

    public void MarkAsSent()
    {
        Sent = true;
        SentAt = DateTime.UtcNow;
        SetUpdatedAt();
    }
}
