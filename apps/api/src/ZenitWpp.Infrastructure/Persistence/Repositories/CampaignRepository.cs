using Microsoft.EntityFrameworkCore;
using ZenitWpp.Domain.Campaigns;
using ZenitWpp.Domain.Campaigns.Repositories;

namespace ZenitWpp.Infrastructure.Persistence.Repositories;

public class CampaignRepository : ICampaignRepository
{
    private readonly AppDbContext _context;

    public CampaignRepository(AppDbContext context) => _context = context;

    public async Task<Campaign?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Campaigns
            .Include(c => c.Recipients)
            .FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<IEnumerable<Campaign>> ListAsync(int page, int pageSize, CancellationToken ct = default)
        => await _context.Campaigns
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task AddAsync(Campaign campaign, CancellationToken ct = default)
    {
        await _context.Campaigns.AddAsync(campaign, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Campaign campaign, CancellationToken ct = default)
    {
        _context.Campaigns.Update(campaign);
        await _context.SaveChangesAsync(ct);
    }
}
