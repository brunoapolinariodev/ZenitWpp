namespace ZenitWpp.Application.Common.Interfaces;

public interface ITotpService
{
    string GenerateSecret();
    string GenerateQrCodeUri(string email, string secret);
    bool ValidateCode(string secret, string code);
}
