using Mapster;
using MediatR;
using ZenitWpp.Application.Agents.DTOs;
using ZenitWpp.Application.Common.Interfaces;
using ZenitWpp.Domain.Agents;
using ZenitWpp.Domain.Agents.Repositories;

namespace ZenitWpp.Application.Agents.Commands.CreateAgent;

public class CreateAgentHandler : IRequestHandler<CreateAgentCommand, AgentResponse>
{
    private readonly IAgentRepository _repository;
    private readonly IPasswordHasher _hasher;

    public CreateAgentHandler(IAgentRepository repository, IPasswordHasher hasher)
    {
        _repository = repository;
        _hasher = hasher;
    }

    public async Task<AgentResponse> Handle(CreateAgentCommand cmd, CancellationToken ct)
    {
        var existing = await _repository.GetByEmailAsync(cmd.Email, ct);
        if (existing is not null)
            throw new InvalidOperationException($"E-mail {cmd.Email} já cadastrado.");

        var passwordHash = _hasher.Hash(cmd.Password);
        var agent = Agent.Create(cmd.Name, cmd.Email, passwordHash, cmd.Role);

        await _repository.AddAsync(agent, ct);
        return agent.Adapt<AgentResponse>();
    }
}
