using MediatR;
using ZenitWpp.Application.Agents.DTOs;

namespace ZenitWpp.Application.Agents.Queries.GetAgent;

public record GetAgentQuery(Guid Id) : IRequest<AgentResponse>;
