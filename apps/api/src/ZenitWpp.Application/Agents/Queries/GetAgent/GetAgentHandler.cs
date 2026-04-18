using Mapster;
using MediatR;
using ZenitWpp.Application.Agents.DTOs;
using ZenitWpp.Domain.Agents.Repositories;

namespace ZenitWpp.Application.Agents.Queries.GetAgent;

public class GetAgentHandler : IRequestHandler<GetAgentQuery, AgentResponse>
{
    private readonly IAgentRepository _repository;

    public GetAgentHandler(IAgentRepository repository)
        => _repository = repository;

    public async Task<AgentResponse> Handle(GetAgentQuery query, CancellationToken ct)
    {
        var agent = await _repository.GetByIdAsync(query.Id, ct)
            ?? throw new KeyNotFoundException($"Agente {query.Id} não encontrado.");

        return agent.Adapt<AgentResponse>();
    }
}
