using CreditManager.Domain.Entities.Credit;
using FluentValidation;

namespace CreditManager.Application.Feature.CreditRequests.Commands.UpdateCreditRequest;

public class UpdateCreditRequestValidator : AbstractValidator<UpdateCreditRequestCommand>
{
    public UpdateCreditRequestValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.Amount).GreaterThan(0);
        RuleFor(c => c.CurrencyCode).Length(3);
        RuleFor(c => c.Comments).MaximumLength(2000);
        RuleFor(c => c.CreditType).Must(value => Enum.IsDefined(typeof(CreditType), value))
            .WithMessage("Invalid credit type.");
        RuleFor(c => c).Must(model =>
        {
            if (model is { PeriodYears: 0, PeriodMonths: 0, PeriodDays: 0 })
            {
                return false;
            }

            return true;
        }).WithMessage("Period can not be empty");
    }
}