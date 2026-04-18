using ZenitWpp.Domain.Automation;

namespace ZenitWpp.Api.Requests.Automation;

public record CreateFlowRequest(string Name, TriggerType TriggerType, string? TriggerValue);
