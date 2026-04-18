using Microsoft.EntityFrameworkCore;
using ZenitWpp.Domain.Agents;
using ZenitWpp.Domain.Agents.Repositories;

namespace ZenitWpp.Infrastructure.Persistence.Repositories;

public class AgentRepository : IAgentRepository
{
    private readonly AppDbContext _context;

    public AgentRepository(AppDbContext context) => _context = context;

    public async Task<Agent?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Agents.FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task<Agent?> GetByEmailAsync(string email, CancellationToken ct = default)
        => await _context.Agents
            .FirstOrDefaultAsync(a => a.Email == email.ToLower(), ct);

    public async Task<IEnumerable<Agent>> ListAsync(CancellationToken ct = default)
        => await _context.Agents.OrderBy(a => a.Name).ToListAsync(ct);

    public async Task AddAsync(Agent agent, CancellationToken ct = default)
    {
        await _context.Agents.AddAsync(agent, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Agent agent, CancellationToken ct = default)
    {
        _context.Agents.Update(agent);
        await _context.SaveChangesAsync(ct);
    }
}
