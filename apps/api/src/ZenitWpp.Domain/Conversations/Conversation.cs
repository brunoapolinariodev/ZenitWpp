using ZenitWpp.Domain.Common;
using ZenitWpp.Domain.Conversations.Events;

namespace ZenitWpp.Domain.Conversations;

public class Conversation : AggregateRoot
{
    private readonly List<Message> _messages = new();

    public Guid ContactId { get; private set; }
    public Guid? AssignedAgentId { get; private set; }
    public ConversationStatus Status { get; private set; }
    public string Channel { get; private set; } = string.Empty;
    public IReadOnlyCollection<Message> Messages => _messages.AsReadOnly();

    private Conversation() { }

    public static Conversation Create(Guid contactId, string channel)
    {
        var conversation = new Conversation
        {
            ContactId = contactId,
            Channel = channel,
            Status = ConversationStatus.Open
        };

        conversation.AddDomainEvent(new ConversationStartedEvent(conversation.Id, contactId));
        return conversation;
    }

    public void SendMessage(string content, MessageType type, MessageDirection direction)
    {
        var message = new Message(Id, content, type, direction);
        _messages.Add(message);
        SetUpdatedAt();
        AddDomainEvent(new MessageSentEvent(Id, message.Id, content, direction));
    }

    public void AssignAgent(Guid agentId)
    {
        AssignedAgentId = agentId;
        Status = ConversationStatus.InProgress;
        SetUpdatedAt();
    }

    public void Transfer(Guid newAgentId)
    {
        var previousAgentId = AssignedAgentId;
        AssignedAgentId = newAgentId;
        Status = ConversationStatus.InProgress;
        SetUpdatedAt();
        AddDomainEvent(new ConversationTransferredEvent(Id, previousAgentId, newAgentId));
    }

    public void Close()
    {
        Status = ConversationStatus.Closed;
        SetUpdatedAt();
        AddDomainEvent(new ConversationClosedEvent(Id, ContactId));
    }

    public void SetWaiting()
    {
        Status = ConversationStatus.Waiting;
        SetUpdatedAt();
    }
}
