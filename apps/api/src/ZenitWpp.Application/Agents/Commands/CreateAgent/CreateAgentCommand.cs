using MediatR;
using ZenitWpp.Application.Agents.DTOs;
using ZenitWpp.Domain.Agents;

namespace ZenitWpp.Application.Agents.Commands.CreateAgent;

public record CreateAgentCommand(string Name, string Email, string Password, AgentRole Role) : IRequest<AgentResponse>;
