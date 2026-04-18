using System.Security.Cryptography;

namespace ZenitWpp.Infrastructure.Auth;

public class TotpService : ITotpService
{
    private const int Step = 30;
    private const int Digits = 6;

    public string GenerateSecret()
    {
        var bytes = RandomNumberGenerator.GetBytes(20);
        return Convert.ToBase64String(bytes).Replace("=", "").Replace("+", "").Replace("/", "")[..32];
    }

    public string GenerateQrCodeUri(string email, string secret)
        => $"otpauth://totp/ZenitWpp:{Uri.EscapeDataString(email)}?secret={secret}&issuer=ZenitWpp&digits={Digits}";

    public bool ValidateCode(string secret, string code)
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() / Step;

        for (var i = -1; i <= 1; i++)
        {
            var expected = GenerateTotp(secret, timestamp + i);
            if (expected == code) return true;
        }

        return false;
    }

    private static string GenerateTotp(string secret, long timestamp)
    {
        var keyBytes = Convert.FromBase64String(PadBase64(secret));
        var timestampBytes = BitConverter.GetBytes(timestamp);

        if (BitConverter.IsLittleEndian)
            Array.Reverse(timestampBytes);

        using var hmac = new HMACSHA1(keyBytes);
        var hash = hmac.ComputeHash(timestampBytes);
        var offset = hash[^1] & 0x0F;
        var code = ((hash[offset] & 0x7F) << 24)
                   | ((hash[offset + 1] & 0xFF) << 16)
                   | ((hash[offset + 2] & 0xFF) << 8)
                   | (hash[offset + 3] & 0xFF);

        return (code % (int)Math.Pow(10, Digits)).ToString().PadLeft(Digits, '0');
    }

    private static string PadBase64(string s)
    {
        return (s.Length % 4) switch
        {
            2 => s + "==",
            3 => s + "=",
            _ => s
        };
    }
}
