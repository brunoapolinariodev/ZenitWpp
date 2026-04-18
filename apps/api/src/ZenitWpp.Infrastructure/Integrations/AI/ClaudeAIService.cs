using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace ZenitWpp.Infrastructure.Integrations.AI;

public class ClaudeAIService : IAIService
{
    private readonly HttpClient _http;
    private readonly string _model;

    public ClaudeAIService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _model = config["Claude:Model"] ?? "claude-sonnet-4-6";
    }

    public async Task<string> AnalyzeSentimentAsync(string text, CancellationToken ct = default)
        => await SendPromptAsync(
            $"Analise o sentimento do seguinte texto e responda apenas com: positivo, negativo ou neutro.\n\nTexto: {text}", ct);

    public async Task<string> SuggestReplyAsync(string conversationHistory, CancellationToken ct = default)
        => await SendPromptAsync(
            $"Você é um assistente de atendimento. Com base no histórico abaixo, sugira uma resposta profissional e empática.\n\nHistórico:\n{conversationHistory}", ct);

    public async Task<string> ClassifyConversationAsync(string text, CancellationToken ct = default)
        => await SendPromptAsync(
            $"Classifique a mensagem abaixo em uma categoria (Suporte, Financeiro, Comercial, Reclamação, Outros) e nível de urgência (alta, média, baixa). Responda em JSON: {{\"category\": \"\", \"urgency\": \"\"}}.\n\nMensagem: {text}", ct);

    private async Task<string> SendPromptAsync(string prompt, CancellationToken ct)
    {
        var payload = new
        {
            model = _model,
            max_tokens = 1024,
            messages = new[] { new { role = "user", content = prompt } }
        };

        var response = await _http.PostAsJsonAsync("/v1/messages", payload, ct);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: ct);
        return json.GetProperty("content")[0].GetProperty("text").GetString() ?? string.Empty;
    }
}
