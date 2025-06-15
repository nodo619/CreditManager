using CreditManager.Domain.Entities.Credit;
using FluentValidation;

namespace CreditManager.Application.Feature.CreditRequests.Commands.CreateCreditRequest;

public class CreateCreditRequestValidator : AbstractValidator<CreateCreditRequestCommand>
{
    public CreateCreditRequestValidator()
    {
        RuleFor(c => c.CustomerId).NotEmpty();
        RuleFor(c => c.Amount).GreaterThan(0);
        RuleFor(c => c.CurrencyCode).Length(3);
        RuleFor(c => c.Comments).MaximumLength(2000);
        RuleFor(c => c.CreditType).Must(value => Enum.IsDefined(typeof(CreditType), value))
            .WithMessage("Invalid credit type.");
    }
}