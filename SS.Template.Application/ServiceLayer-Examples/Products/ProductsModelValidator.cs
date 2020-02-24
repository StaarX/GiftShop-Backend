using FluentValidation;
using SS.Template.Core;

namespace SS.Template.Application.Products
{
    public sealed class ProductsModelValidator : AbstractValidator<ProductsModel>
    {
        public ProductsModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Matches(Regex.LettersNSpaces)
                .MaximumLength(AppConstants.StandardValueLength);
            RuleFor(x => x.Description)
                .Matches(Regex.Desc)
                .MaximumLength(AppConstants.StandardValueLength);

        }
    }
}
