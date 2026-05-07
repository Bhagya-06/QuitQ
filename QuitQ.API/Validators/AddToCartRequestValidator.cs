using FluentValidation;
using QuitQ.API.DTOs.Request;

namespace QuitQ.API.Validators
{
    public class AddToCartRequestValidator : AbstractValidator<AddToCartRequest>
    {
        public AddToCartRequestValidator()
        {
            RuleFor(x => x.ProductId).GreaterThan(0);

            RuleFor(x => x.Quantity).GreaterThan(0);
        }
    }
}
