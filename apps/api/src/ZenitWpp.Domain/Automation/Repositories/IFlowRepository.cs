namespace ZenitWpp.Domain.Automation.Repositories;

public interface IFlowRepository
{
    Task<Flow?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Flow>> ListActiveAsync(CancellationToken ct = default);
    Task<Flow?> FindByTriggerAsync(TriggerType type, string? value, CancellationToken ct = default);
    Task AddAsync(Flow flow, CancellationToken ct = default);
    Task UpdateAsync(Flow flow, CancellationToken ct = default);
}
