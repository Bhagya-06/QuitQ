using FluentValidation;
using QuitQ.API.DTOs.Request;

namespace QuitQ.API.Validators
{
    public class AddProductRequestValidator : AbstractValidator<AddProductRequest>
    {
        public AddProductRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();

            RuleFor(x => x.Description).NotEmpty();

            RuleFor(x => x.Price).GreaterThan(0);

            RuleFor(x => x.Stock).GreaterThanOrEqualTo(0);

            RuleFor(x => x.CategoryId).GreaterThan(0);
        }
    }
}
