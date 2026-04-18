using MediatR;
using ZenitWpp.Application.Conversations.DTOs;

namespace ZenitWpp.Application.Conversations.Commands.CreateConversation;

public record CreateConversationCommand(Guid ContactId, string Channel) : IRequest<ConversationResponse>;
