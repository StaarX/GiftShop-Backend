using FluentValidation;
using SS.Template.Core;

namespace SS.Template.Application.Orders
{
    public sealed class ProductsModelValidator : AbstractValidator<OrdersModel>
    {
        public ProductsModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(AppConstants.StandardValueLength);
        }
    }
}
