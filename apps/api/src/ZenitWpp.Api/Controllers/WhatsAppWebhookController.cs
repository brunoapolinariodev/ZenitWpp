using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ZenitWpp.Api.Hubs;

namespace ZenitWpp.Api.Controllers;

[ApiController]
[Route("api/webhook/whatsapp")]
public class WhatsAppWebhookController : ControllerBase
{
    private readonly IHubContext<ChatHub> _hub;
    private readonly ILogger<WhatsAppWebhookController> _logger;

    public WhatsAppWebhookController(IHubContext<ChatHub> hub, ILogger<WhatsAppWebhookController> logger)
    {
        _hub = hub;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Receive([FromBody] object payload)
    {
        _logger.LogInformation("Webhook recebido: {Payload}", payload);

        await _hub.Clients.All.SendAsync("NewMessage", payload);

        return Ok();
    }
}
