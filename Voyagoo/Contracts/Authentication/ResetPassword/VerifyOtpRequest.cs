namespace Voyagoo.Contracts.Authentication.ResetPassword
{
    public record VerifyOtpRequest(string Email, string Code);
}
