using Microsoft.EntityFrameworkCore;
using ZenitWpp.Domain.Automation;
using ZenitWpp.Domain.Automation.Repositories;

namespace ZenitWpp.Infrastructure.Persistence.Repositories;

public class FlowRepository : IFlowRepository
{
    private readonly AppDbContext _context;

    public FlowRepository(AppDbContext context) => _context = context;

    public async Task<Flow?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Flows
            .Include(f => f.Steps)
            .FirstOrDefaultAsync(f => f.Id == id, ct);

    public async Task<IEnumerable<Flow>> ListActiveAsync(CancellationToken ct = default)
        => await _context.Flows
            .Include(f => f.Steps)
            .Where(f => f.IsActive)
            .ToListAsync(ct);

    public async Task<Flow?> FindByTriggerAsync(TriggerType type, string? value, CancellationToken ct = default)
        => await _context.Flows
            .Include(f => f.Steps)
            .FirstOrDefaultAsync(f => f.IsActive && f.TriggerType == type && f.TriggerValue == value, ct);

    public async Task AddAsync(Flow flow, CancellationToken ct = default)
    {
        await _context.Flows.AddAsync(flow, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Flow flow, CancellationToken ct = default)
    {
        _context.Flows.Update(flow);
        await _context.SaveChangesAsync(ct);
    }
}
