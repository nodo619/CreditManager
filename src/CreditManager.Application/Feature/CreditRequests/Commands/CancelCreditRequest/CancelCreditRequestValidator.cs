using FluentValidation;

namespace CreditManager.Application.Feature.CreditRequests.Commands.CancelCreditRequest;

public class CancelCreditRequestValidator : AbstractValidator<CancelCreditRequestCommand>
{
    public CancelCreditRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");
    }
} 