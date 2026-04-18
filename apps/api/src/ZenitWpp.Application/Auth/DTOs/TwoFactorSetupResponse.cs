namespace ZenitWpp.Application.Auth.DTOs;

public record TwoFactorSetupResponse(string Secret, string QrCodeUri);
