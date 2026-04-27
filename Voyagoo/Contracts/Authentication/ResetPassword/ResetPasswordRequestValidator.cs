using FluentValidation;
using Voyagoo.Abstractions.Consts;

namespace Voyagoo.Contracts.Authentication.ResetPassword
{
    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .Matches(RegexPatterns.Password)
                .WithMessage("Password should be at least 8 digits and should contain Lowercase, NonAlphanumeric and Uppercase");

            RuleFor(x => x.ConfirmNewPassword)
                .NotEmpty()
                .Equal(x => x.NewPassword)
                .WithMessage("Passwords do not match");
        }
    }
}
