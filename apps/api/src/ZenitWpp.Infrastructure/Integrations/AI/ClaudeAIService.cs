using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using ZenitWpp.Application.Common.Interfaces;

namespace ZenitWpp.Infrastructure.Integrations.AI;

public class AiMicroserviceClient : IAIService
{
    private readonly HttpClient _http;

    public AiMicroserviceClient(HttpClient http, IConfiguration config)
    {
        _http = http;
        _http.DefaultRequestHeaders.Add("X-API-Key", config["AiService:ApiKey"]);
    }

    public async Task<string> AnalyzeSentimentAsync(string text, string language = "pt", CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync("/ai/sentiment", new { text, language }, ct);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: ct);
        return json.GetProperty("label").GetString() ?? string.Empty;
    }

    public async Task<string> SuggestReplyAsync(string conversationHistory, string language = "pt", CancellationToken ct = default)
    {
        var payload = new
        {
            conversation_history = new[]
            {
                new { role = "customer", content = conversationHistory }
            },
            language
        };

        var response = await _http.PostAsJsonAsync("/ai/suggestion", payload, ct);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: ct);
        var suggestions = json.GetProperty("suggestions");
        return suggestions.GetArrayLength() > 0
            ? suggestions[0].GetString() ?? string.Empty
            : string.Empty;
    }

    public async Task<string> ClassifyConversationAsync(string text, string language = "pt", CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync("/ai/classification", new { text, language }, ct);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: ct);
        return json.GetRawText();
    }
}
