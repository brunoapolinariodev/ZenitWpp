using MediatR;
using ZenitWpp.Domain.Agents.Repositories;

namespace ZenitWpp.Application.Agents.Commands.UpdateAgentStatus;

public class UpdateAgentStatusHandler : IRequestHandler<UpdateAgentStatusCommand>
{
    private readonly IAgentRepository _repository;

    public UpdateAgentStatusHandler(IAgentRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateAgentStatusCommand cmd, CancellationToken ct)
    {
        var agent = await _repository.GetByIdAsync(cmd.AgentId, ct)
            ?? throw new KeyNotFoundException($"Agente {cmd.AgentId} não encontrado.");

        agent.SetOnlineStatus(cmd.IsOnline);
        await _repository.UpdateAsync(agent, ct);
    }
}
