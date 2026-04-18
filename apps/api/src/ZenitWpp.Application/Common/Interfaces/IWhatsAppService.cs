namespace ZenitWpp.Application.Common.Interfaces;

public interface IWhatsAppService
{
    Task SendTextAsync(string phone, string message, CancellationToken ct = default);
    Task SendMediaAsync(string phone, string mediaUrl, string caption, CancellationToken ct = default);
    Task SendButtonsAsync(string phone, string body, IEnumerable<string> buttons, CancellationToken ct = default);
}
