using Microsoft.EntityFrameworkCore;
using ZenitWpp.Domain.Conversations;
using ZenitWpp.Domain.Conversations.Repositories;

namespace ZenitWpp.Infrastructure.Persistence.Repositories;

public class ConversationRepository : IConversationRepository
{
    private readonly AppDbContext _context;

    public ConversationRepository(AppDbContext context) => _context = context;

    public async Task<Conversation?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Conversations
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<IEnumerable<Conversation>> ListAsync(int page, int pageSize, CancellationToken ct = default)
        => await _context.Conversations
            .OrderByDescending(c => c.UpdatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task AddAsync(Conversation conversation, CancellationToken ct = default)
    {
        await _context.Conversations.AddAsync(conversation, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Conversation conversation, CancellationToken ct = default)
    {
        _context.Conversations.Update(conversation);
        await _context.SaveChangesAsync(ct);
    }
}
