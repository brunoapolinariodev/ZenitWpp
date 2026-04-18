namespace ZenitWpp.Infrastructure.Integrations.AI;

public interface IAIService
{
    Task<string> AnalyzeSentimentAsync(string text, CancellationToken ct = default);
    Task<string> SuggestReplyAsync(string conversationHistory, CancellationToken ct = default);
    Task<string> ClassifyConversationAsync(string text, CancellationToken ct = default);
}
