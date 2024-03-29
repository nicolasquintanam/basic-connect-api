namespace BasicConnectApi.Services;

public interface IOtpService
{
    Task<string> GenerateOtp(string? email, string? context);
    Task<bool> ValidateOtp(string? email, string? otp, string? context);
    Task<string> GenerateTemporaryAccessToken(string? email, string? context);
}