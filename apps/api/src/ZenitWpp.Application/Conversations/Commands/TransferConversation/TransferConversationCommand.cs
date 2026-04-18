using MediatR;
using ZenitWpp.Application.Conversations.DTOs;

namespace ZenitWpp.Application.Conversations.Commands.TransferConversation;

public record TransferConversationCommand(Guid ConversationId, Guid NewAgentId) : IRequest<ConversationResponse>;
