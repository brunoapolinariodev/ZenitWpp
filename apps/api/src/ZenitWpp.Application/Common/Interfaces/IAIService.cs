namespace ZenitWpp.Application.Common.Interfaces;

public interface IAIService
{
    Task<string> AnalyzeSentimentAsync(string text, string language = "pt", CancellationToken ct = default);
    Task<string> SuggestReplyAsync(string conversationHistory, string language = "pt", CancellationToken ct = default);
    Task<string> ClassifyConversationAsync(string text, string language = "pt", CancellationToken ct = default);
}
