using FluentValidation;

namespace Voyagoo.Contracts.Authentication.ResetPassword
{
    public class VerifyOtpRequestValidator : AbstractValidator<VerifyOtpRequest>
    {
        public VerifyOtpRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Code)
                .NotEmpty()
                .Length(6);
        }
    }
}
