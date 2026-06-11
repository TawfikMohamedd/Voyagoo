using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Cryptography;
using Voyagoo.Abstractions;
using Voyagoo.Abstractions.Consts;
using Voyagoo.Authentication;
using Voyagoo.Contracts.Authentication;
using Voyagoo.Entities;
using Voyagoo.Errors;

namespace Voyagoo.Services
{
    public class AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider,IEmailSender emailSender) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IJwtProvider jwtProvider = jwtProvider;
        private readonly IEmailSender emailSender = emailSender;
        private readonly int _refreshTokenExpiryDays = 14;

        public async Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            //check Email

            var user = await _userManager.FindByEmailAsync(email);
            
            if (user == null)
                return null;

            //check Password

            var isValidPassword = await _userManager.CheckPasswordAsync(user, password);
            
            if (!isValidPassword)
                return null;


            if (!user.IsActive)
                return null;

            //generate Jwt token

            var roles = await _userManager.GetRolesAsync(user);
            var (token, expiresIn) = jwtProvider.GenerateToken(user, roles);

            //generate Jwt refresh token

            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                ExpiresOn = refreshTokenExpiration
            });

            await _userManager.UpdateAsync(user);

            //return new auth response

            return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn,refreshToken,refreshTokenExpiration, roles);

            
        }

        public async Task<AuthResponse?> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {
            var userId = jwtProvider.ValidateToken(token);
            if (userId is null)
                return null;

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return null;

            var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);
            if (userRefreshToken is null)
                return null;

            userRefreshToken.RevokedOn = DateTime.UtcNow;

            // ✅ جيب الـ roles
            var roles = await _userManager.GetRolesAsync(user);

            // ✅ بعّت الـ roles للـ GenerateToken
            var (newToken, expiresIn) = jwtProvider.GenerateToken(user, roles);

            var newRefreshToken = GenerateRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = newRefreshToken,
                ExpiresOn = refreshTokenExpiration
            });

            await _userManager.UpdateAsync(user);

            return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, newToken, expiresIn, newRefreshToken, refreshTokenExpiration,roles);
        }

        public async Task<bool> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {
            var userId = jwtProvider.ValidateToken(token);

            if (userId is null)
                return false;

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return false;

            var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

            if (userRefreshToken is null)
                return false;

            userRefreshToken.RevokedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            return true;
        }


        //public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
        







        private static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public async Task<Result<AuthResponse>> RegisterAsync(Contracts.Authentication.Register.RegisterRequest request, CancellationToken cancellationToken = default)
        {
            {
                var emailIsExists = await _userManager.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);

                if (emailIsExists)
                    return Result.Failure<AuthResponse>(UserErrors.DuplicatedEmail);

                var user = request.Adapt<ApplicationUser>();

                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var (token, expiresIn) = jwtProvider.GenerateToken(user, roles);

                    //generate Jwt refresh token

                    var refreshToken = GenerateRefreshToken();
                    var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

                    user.RefreshTokens.Add(new RefreshToken
                    {
                        Token = refreshToken,
                        ExpiresOn = refreshTokenExpiration
                    });

                    await _userManager.UpdateAsync(user);



                    var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn, refreshToken, refreshTokenExpiration, roles);

                    await _userManager.AddToRoleAsync(user, DefaultRoles.Member);
                    return Result.Success(response);

                }

                var error = result.Errors.First();

                return Result.Failure<AuthResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

            }
        }


        //hollaaaaa





        public async Task<Result> SendResetPasswordOtpAsync(string email, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                return Result.Failure(UserErrors.EmailNotFound);

            // Generate 6-digit OTP
            var otp = Random.Shared.Next(100000, 999999).ToString();
            var otpExpiry = DateTime.UtcNow.AddMinutes(10);

            user.PasswordResetOtp = otp;
            user.PasswordResetOtpExpiry = otpExpiry;
            user.IsOtpVerified = false;

            await _userManager.UpdateAsync(user);

            // Send Email
            var emailBody = EmailTemplates.GetOtpEmailTemplate(user.FirstName, otp);
            await emailSender.SendEmailAsync(user.Email!, "Voyagoo - Password Reset OTP", emailBody);

            return Result.Success();
        }

        public async Task<Result> VerifyOtpAsync(string email, string code, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                return Result.Failure(UserErrors.EmailNotFound);

            // تشيك علي الـ OTP و expiry
            var isValidOtp = user.PasswordResetOtp == code
                             && user.PasswordResetOtpExpiry.HasValue
                             && user.PasswordResetOtpExpiry > DateTime.UtcNow;

            if (!isValidOtp)
                return Result.Failure(UserErrors.InvalidOrExpiredOtp);

            // علم إن الـ OTP اتتحقق منه
            user.IsOtpVerified = true;

            await _userManager.UpdateAsync(user);

            return Result.Success();
        }

        public async Task<Result> ResetPasswordAsync(string email, string newPassword, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                return Result.Failure(UserErrors.EmailNotFound);

            // تأكد إن الـ OTP اتتحقق منه الأول
            if (!user.IsOtpVerified)
                return Result.Failure(UserErrors.OtpNotVerified);

            // 🔐 تغيير الباسورد بدون token
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, newPassword);

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                var error = updateResult.Errors.First();
                return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
            }

            // 🧹 تنظيف بيانات الـ OTP
            user.PasswordResetOtp = null;
            user.PasswordResetOtpExpiry = null;
            user.IsOtpVerified = false;

            await _userManager.UpdateAsync(user);

            return Result.Success();
        }






    }
}
