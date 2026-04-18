using Mapster;
using MediatR;
using ZenitWpp.Application.Conversations.DTOs;
using ZenitWpp.Domain.Conversations.Repositories;

namespace ZenitWpp.Application.Conversations.Queries.GetConversation;

public class GetConversationHandler : IRequestHandler<GetConversationQuery, ConversationResponse>
{
    private readonly IConversationRepository _repository;

    public GetConversationHandler(IConversationRepository repository)
        => _repository = repository;

    public async Task<ConversationResponse> Handle(GetConversationQuery query, CancellationToken ct)
    {
        var conversation = await _repository.GetByIdAsync(query.Id, ct)
            ?? throw new KeyNotFoundException($"Conversa {query.Id} não encontrada.");

        return conversation.Adapt<ConversationResponse>();
    }
}
