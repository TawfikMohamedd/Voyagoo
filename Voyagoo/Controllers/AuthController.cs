using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Voyagoo.Abstractions;
using Voyagoo.Authentication;
using Voyagoo.Contracts.Authentication;
using Voyagoo.Contracts.Authentication.Register;
using Voyagoo.Contracts.Authentication.ResetPassword;
using Voyagoo.Services;

namespace Voyagoo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService ) : ControllerBase
    {
        private readonly IAuthService _authService = authService;




        [HttpPost("")]
        public async Task<IActionResult> Login([FromBody]LoginRequest request, CancellationToken cancellationToken)
        {

            var authResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);

            return authResult is null ? BadRequest("Invalid email/phone or password.") : Ok(authResult);
        
        
        }





        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {

            var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

            return authResult is null ? BadRequest("Invalid token") : Ok(authResult);


        }




        [HttpPost("revoke-refresh-token")]
        public async Task<IActionResult> RevokeRefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {

            var isRevoked = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

            return isRevoked ? Ok() : BadRequest("Operation faild");


        }





        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
        {

            var result = await _authService.RegisterAsync(request, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();


        }





        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request, CancellationToken cancellationToken)
        {
            var result = await _authService.SendResetPasswordOtpAsync(request.Email, cancellationToken);

            return result.IsSuccess ? Ok("OTP has been sent to your email") : result.ToProblem();
        }




        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request, CancellationToken cancellationToken)
        {
            var result = await _authService.VerifyOtpAsync(request.Email, request.Code, cancellationToken);

            return result.IsSuccess ? Ok("OTP verified successfully") : result.ToProblem();
        }





        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            var result = await _authService.ResetPasswordAsync(request.Email, request.NewPassword, cancellationToken);

            return result.IsSuccess ? Ok("Password has been reset successfully") : result.ToProblem();
        }





    }
}
