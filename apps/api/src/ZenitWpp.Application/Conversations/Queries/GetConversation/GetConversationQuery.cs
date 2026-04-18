using MediatR;
using ZenitWpp.Application.Conversations.DTOs;

namespace ZenitWpp.Application.Conversations.Queries.GetConversation;

public record GetConversationQuery(Guid Id) : IRequest<ConversationResponse>;
