namespace ZenitWpp.Domain.Conversations.Repositories;

public interface IConversationRepository
{
    Task<Conversation?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Conversation>> ListAsync(int page, int pageSize, CancellationToken ct = default);
    Task AddAsync(Conversation conversation, CancellationToken ct = default);
    Task UpdateAsync(Conversation conversation, CancellationToken ct = default);
}
