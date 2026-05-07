using FluentValidation;
using QuitQ.API.DTOs.Request;

namespace QuitQ.API.Validators
{
    public class AddAddressRequestValidator : AbstractValidator<AddAddressRequest>
    {
        public AddAddressRequestValidator()
        {
            RuleFor(x => x.AddressName).NotEmpty();

            RuleFor(x => x.Address1).NotEmpty();

            RuleFor(x => x.City).NotEmpty();

            RuleFor(x => x.Pincode).Matches(@"^[0-9]{6}$").WithMessage("Invalid pincode");
        }
    }
}
