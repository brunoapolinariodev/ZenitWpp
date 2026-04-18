using ZenitWpp.Domain.Conversations;

namespace ZenitWpp.Api.Requests.Conversations;

public record SendMessageRequest(string Content, MessageType Type, MessageDirection Direction);
