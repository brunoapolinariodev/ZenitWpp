using MediatR;
using ZenitWpp.Application.Automation.DTOs;
using ZenitWpp.Domain.Automation;

namespace ZenitWpp.Application.Automation.Commands.CreateFlow;

public record CreateFlowCommand(string Name, TriggerType TriggerType, string? TriggerValue) : IRequest<FlowResponse>;
