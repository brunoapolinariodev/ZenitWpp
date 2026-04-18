using MediatR;
using ZenitWpp.Application.Conversations.DTOs;

namespace ZenitWpp.Application.Conversations.Queries.ListConversations;

public record ListConversationsQuery(int Page = 1, int PageSize = 20) : IRequest<IEnumerable<ConversationResponse>>;
