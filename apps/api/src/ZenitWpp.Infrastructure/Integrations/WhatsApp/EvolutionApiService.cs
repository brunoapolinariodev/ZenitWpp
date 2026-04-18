using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using ZenitWpp.Application.Common.Interfaces;

namespace ZenitWpp.Infrastructure.Integrations.WhatsApp;

public class EvolutionApiService : IWhatsAppService
{
    private readonly HttpClient _http;
    private readonly string _instance;

    public EvolutionApiService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _instance = config["EvolutionApi:Instance"]!;
    }

    public async Task SendTextAsync(string phone, string message, CancellationToken ct = default)
    {
        var payload = new { number = phone, text = message };
        await _http.PostAsJsonAsync($"/message/sendText/{_instance}", payload, ct);
    }

    public async Task SendMediaAsync(string phone, string mediaUrl, string caption, CancellationToken ct = default)
    {
        var payload = new { number = phone, mediaUrl, caption };
        await _http.PostAsJsonAsync($"/message/sendMedia/{_instance}", payload, ct);
    }

    public async Task SendButtonsAsync(string phone, string body, IEnumerable<string> buttons, CancellationToken ct = default)
    {
        var payload = new
        {
            number = phone,
            buttonMessage = new
            {
                contentText = body,
                buttons = buttons.Select((b, i) => new { buttonId = $"btn_{i}", buttonText = new { displayText = b } })
            }
        };
        await _http.PostAsJsonAsync($"/message/sendButtons/{_instance}", payload, ct);
    }
}
