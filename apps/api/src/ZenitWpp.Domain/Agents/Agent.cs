using ZenitWpp.Domain.Common;
using ZenitWpp.Domain.Agents.Events;

namespace ZenitWpp.Domain.Agents;

public class Agent : AggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public AgentRole Role { get; private set; }
    public bool IsOnline { get; private set; }
    public bool TwoFactorEnabled { get; private set; }
    public string? TwoFactorSecret { get; private set; }

    private Agent() { }

    public static Agent Create(string name, string email, string passwordHash, AgentRole role = AgentRole.Operator)
    {
        return new Agent
        {
            Name = name,
            Email = email.Trim().ToLower(),
            PasswordHash = passwordHash,
            Role = role,
            IsOnline = false,
            TwoFactorEnabled = false
        };
    }

    public void SetOnlineStatus(bool isOnline)
    {
        IsOnline = isOnline;
        SetUpdatedAt();
        AddDomainEvent(new AgentStatusChangedEvent(Id, isOnline));
    }

    public void EnableTwoFactor(string secret)
    {
        TwoFactorEnabled = true;
        TwoFactorSecret = secret;
        SetUpdatedAt();
    }

    public void UpdateProfile(string name)
    {
        Name = name;
        SetUpdatedAt();
    }
}
