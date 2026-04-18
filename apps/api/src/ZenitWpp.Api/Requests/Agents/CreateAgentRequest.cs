using ZenitWpp.Domain.Agents;

namespace ZenitWpp.Api.Requests.Agents;

public record CreateAgentRequest(string Name, string Email, string Password, AgentRole Role);
