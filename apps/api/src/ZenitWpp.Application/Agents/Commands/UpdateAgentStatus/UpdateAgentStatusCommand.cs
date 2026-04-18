using MediatR;

namespace ZenitWpp.Application.Agents.Commands.UpdateAgentStatus;

public record UpdateAgentStatusCommand(Guid AgentId, bool IsOnline) : IRequest;
