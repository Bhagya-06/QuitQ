using FluentValidation;
using QuitQ.API.DTOs.Request;

namespace QuitQ.API.Validators
{
    public class PlaceOrderRequestValidator : AbstractValidator<PlaceOrderRequest>
    {
        private readonly string[] validMethods = { "UPI", "Card", "Netbanking", "Cash on Delivery" };

        public PlaceOrderRequestValidator()
        {
            RuleFor(x => x.AddressId).GreaterThan(0);

            RuleFor(x => x.PaymentMethod).NotEmpty().IsInEnum().WithMessage("Invalid payment method. Your options are UPI, Card, Netbanking and CashOnDelivery");
        }
    }
}
