namespace ZenitWpp.Api.Requests.Conversations;

public record CreateConversationRequest(Guid ContactId, string Channel);
