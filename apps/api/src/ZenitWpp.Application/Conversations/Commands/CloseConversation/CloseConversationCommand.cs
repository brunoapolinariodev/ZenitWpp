using MediatR;

namespace ZenitWpp.Application.Conversations.Commands.CloseConversation;

public record CloseConversationCommand(Guid ConversationId) : IRequest;
