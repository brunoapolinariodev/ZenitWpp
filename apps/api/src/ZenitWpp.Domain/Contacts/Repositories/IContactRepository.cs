namespace ZenitWpp.Domain.Contacts.Repositories;

public interface IContactRepository
{
    Task<Contact?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Contact?> GetByPhoneAsync(string phone, CancellationToken ct = default);
    Task<IEnumerable<Contact>> ListAsync(int page, int pageSize, CancellationToken ct = default);
    Task AddAsync(Contact contact, CancellationToken ct = default);
    Task UpdateAsync(Contact contact, CancellationToken ct = default);
}
