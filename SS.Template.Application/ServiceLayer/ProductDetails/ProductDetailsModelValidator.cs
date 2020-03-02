using FluentValidation;
using SS.Template.Core;

namespace SS.Template.Application.ProductDetail
{
    public sealed class ProductDetailsModelValidator : AbstractValidator<ProductDetailsModel>
    {
        public ProductDetailsModelValidator()
        {
            RuleFor(x => x.Type)
                .NotEmpty()
                .MaximumLength(AppConstants.StandardValueLength);

            RuleFor(x => x.Price)
                .NotEmpty()
                .GreaterThan(default(decimal));
            RuleFor(x => x.Availability)
                .NotEmpty();
        }
    }
}
