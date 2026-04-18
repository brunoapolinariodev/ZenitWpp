using ZenitWpp.Domain.Automation;

namespace ZenitWpp.Application.Automation.DTOs;

public record FlowResponse(
    Guid Id,
    string Name,
    TriggerType TriggerType,
    string? TriggerValue,
    bool IsActive,
    DateTime CreatedAt
);
