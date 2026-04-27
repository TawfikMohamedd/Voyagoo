namespace Voyagoo.Contracts.Authentication.ResetPassword
{
    public record ResetPasswordRequest(string Email, string NewPassword, string ConfirmNewPassword);
}
