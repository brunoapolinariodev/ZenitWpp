using MediatR;
using ZenitWpp.Application.Conversations.DTOs;
using ZenitWpp.Domain.Conversations;

namespace ZenitWpp.Application.Conversations.Commands.UploadMedia;

public record UploadMediaCommand(
    Guid ConversationId,
    Stream Content,
    string FileName,
    string ContentType,
    MessageDirection Direction
) : IRequest<MessageResponse>;
