using FluentValidation;

namespace Voyagoo.Contracts.Restaurants
{
    public class AddCommentRequestValidator : AbstractValidator<AddCommentRequest>
    {
        public AddCommentRequestValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .Length(3, 1000);

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5)
                .WithMessage("Rating must be between 1 and 5");
        }
    }
}
