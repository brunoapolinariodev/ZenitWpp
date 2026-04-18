using ZenitWpp.Domain.Common;

namespace ZenitWpp.Domain.Agents.Events;

public class AgentStatusChangedEvent : BaseDomainEvent
{
    public Guid AgentId { get; }
    public bool IsOnline { get; }

    public AgentStatusChangedEvent(Guid agentId, bool isOnline)
    {
        AgentId = agentId;
        IsOnline = isOnline;
    }
}
