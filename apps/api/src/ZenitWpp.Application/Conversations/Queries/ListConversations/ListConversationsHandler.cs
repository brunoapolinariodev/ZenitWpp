using Mapster;
using MediatR;
using ZenitWpp.Application.Conversations.DTOs;
using ZenitWpp.Domain.Conversations.Repositories;

namespace ZenitWpp.Application.Conversations.Queries.ListConversations;

public class ListConversationsHandler : IRequestHandler<ListConversationsQuery, IEnumerable<ConversationResponse>>
{
    private readonly IConversationRepository _repository;

    public ListConversationsHandler(IConversationRepository repository)
        => _repository = repository;

    public async Task<IEnumerable<ConversationResponse>> Handle(ListConversationsQuery query, CancellationToken ct)
    {
        var conversations = await _repository.ListAsync(query.Page, query.PageSize, ct);
        return conversations.Adapt<IEnumerable<ConversationResponse>>();
    }
}
