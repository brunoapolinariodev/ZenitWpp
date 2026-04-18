using Microsoft.EntityFrameworkCore;
using ZenitWpp.Domain.Contacts;
using ZenitWpp.Domain.Contacts.Repositories;

namespace ZenitWpp.Infrastructure.Persistence.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly AppDbContext _context;

    public ContactRepository(AppDbContext context) => _context = context;

    public async Task<Contact?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Contacts.FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<Contact?> GetByPhoneAsync(string phone, CancellationToken ct = default)
        => await _context.Contacts
            .FirstOrDefaultAsync(c => c.PhoneNumber.Value == phone, ct);

    public async Task<IEnumerable<Contact>> ListAsync(int page, int pageSize, CancellationToken ct = default)
        => await _context.Contacts
            .OrderBy(c => c.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task AddAsync(Contact contact, CancellationToken ct = default)
    {
        await _context.Contacts.AddAsync(contact, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Contact contact, CancellationToken ct = default)
    {
        _context.Contacts.Update(contact);
        await _context.SaveChangesAsync(ct);
    }
}
