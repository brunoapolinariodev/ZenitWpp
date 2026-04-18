using Microsoft.EntityFrameworkCore;
using ZenitWpp.Domain.Notifications;
using ZenitWpp.Domain.Notifications.Repositories;

namespace ZenitWpp.Infrastructure.Persistence.Repositories;

public class ReminderRepository : IReminderRepository
{
    private readonly AppDbContext _context;

    public ReminderRepository(AppDbContext context) => _context = context;

    public async Task<Reminder?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Reminders.FirstOrDefaultAsync(r => r.Id == id, ct);

    public async Task<IEnumerable<Reminder>> ListPendingAsync(CancellationToken ct = default)
        => await _context.Reminders
            .Where(r => !r.Sent && r.ScheduledAt <= DateTime.UtcNow)
            .ToListAsync(ct);

    public async Task AddAsync(Reminder reminder, CancellationToken ct = default)
    {
        await _context.Reminders.AddAsync(reminder, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Reminder reminder, CancellationToken ct = default)
    {
        _context.Reminders.Update(reminder);
        await _context.SaveChangesAsync(ct);
    }
}
