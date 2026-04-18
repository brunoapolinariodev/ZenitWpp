namespace ZenitWpp.Domain.Notifications.Repositories;

public interface IReminderRepository
{
    Task<Reminder?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Reminder>> ListPendingAsync(CancellationToken ct = default);
    Task AddAsync(Reminder reminder, CancellationToken ct = default);
    Task UpdateAsync(Reminder reminder, CancellationToken ct = default);
}
