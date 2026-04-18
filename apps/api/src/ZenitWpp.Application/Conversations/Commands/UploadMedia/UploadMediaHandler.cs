using Mapster;
using MediatR;
using ZenitWpp.Application.Common.Interfaces;
using ZenitWpp.Application.Conversations.DTOs;
using ZenitWpp.Domain.Conversations;
using ZenitWpp.Domain.Conversations.Repositories;

namespace ZenitWpp.Application.Conversations.Commands.UploadMedia;

public class UploadMediaHandler : IRequestHandler<UploadMediaCommand, MessageResponse>
{
    private readonly IConversationRepository _repository;
    private readonly IStorageService _storage;

    public UploadMediaHandler(IConversationRepository repository, IStorageService storage)
    {
        _repository = repository;
        _storage = storage;
    }

    public async Task<MessageResponse> Handle(UploadMediaCommand cmd, CancellationToken ct)
    {
        var conversation = await _repository.GetByIdAsync(cmd.ConversationId, ct)
            ?? throw new KeyNotFoundException($"Conversa {cmd.ConversationId} não encontrada.");

        var mediaUrl = await _storage.UploadAsync(cmd.Content, cmd.FileName, cmd.ContentType, ct);
        var messageType = ResolveMessageType(cmd.ContentType);

        conversation.SendMessage(mediaUrl, messageType, cmd.Direction);
        await _repository.UpdateAsync(conversation, ct);

        return conversation.Messages.Last().Adapt<MessageResponse>();
    }

    private static MessageType ResolveMessageType(string contentType) => contentType switch
    {
        var t when t.StartsWith("image/") => MessageType.Image,
        var t when t.StartsWith("audio/") => MessageType.Audio,
        var t when t.StartsWith("video/") => MessageType.Video,
        _ => MessageType.Document
    };
}
