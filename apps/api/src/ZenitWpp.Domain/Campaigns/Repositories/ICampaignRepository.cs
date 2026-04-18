namespace ZenitWpp.Domain.Campaigns.Repositories;

public interface ICampaignRepository
{
    Task<Campaign?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Campaign>> ListAsync(int page, int pageSize, CancellationToken ct = default);
    Task AddAsync(Campaign campaign, CancellationToken ct = default);
    Task UpdateAsync(Campaign campaign, CancellationToken ct = default);
}
