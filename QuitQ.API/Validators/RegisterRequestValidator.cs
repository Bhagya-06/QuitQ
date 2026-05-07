using FluentValidation;
using QuitQ.API.DTOs.Request;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MinimumLength(3);

        RuleFor(x => x.Name).NotEmpty();

        RuleFor(x => x.Email).NotEmpty().EmailAddress();

        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);

        RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Passwords do not match");

        RuleFor(x => x.Phone).NotEmpty().Matches(@"^[0-9]{10}$").WithMessage("Phone must be 10 digits");

        RuleFor(x => x.Address).NotEmpty();

        RuleFor(x => x.City).NotEmpty();

        RuleFor(x => x.PinCode).NotEmpty().Matches(@"^[0-9]{6}$").WithMessage("Invalid pincode");

        RuleFor(x => x.Role).Must(role => new[] { "Buyer", "Seller", "Admin" }.Contains(role)).WithMessage("Invalid role");

        RuleFor(x => x.StoreName).NotEmpty().When(x => x.Role == "Seller").WithMessage("StoreName is required for Seller");
    }
}
