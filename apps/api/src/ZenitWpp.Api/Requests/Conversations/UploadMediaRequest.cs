using ZenitWpp.Domain.Conversations;

namespace ZenitWpp.Api.Requests.Conversations;

public record UploadMediaRequest(IFormFile File, MessageDirection Direction);
