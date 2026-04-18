using ZenitWpp.Domain.Agents;

namespace ZenitWpp.Application.Agents.DTOs;

public record AgentResponse(
    Guid Id,
    string Name,
    string Email,
    AgentRole Role,
    bool IsOnline,
    bool TwoFactorEnabled,
    DateTime CreatedAt
);
