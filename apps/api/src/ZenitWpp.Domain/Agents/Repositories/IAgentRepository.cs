namespace ZenitWpp.Domain.Agents.Repositories;

public interface IAgentRepository
{
    Task<Agent?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Agent?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<IEnumerable<Agent>> ListAsync(CancellationToken ct = default);
    Task AddAsync(Agent agent, CancellationToken ct = default);
    Task UpdateAsync(Agent agent, CancellationToken ct = default);
}
