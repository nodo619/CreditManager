using FluentValidation;

namespace CreditManager.Application.Feature.CreditRequests.Commands.RejectCreditRequest;

public class RejectCreditRequestValidator : AbstractValidator<RejectCreditRequestCommand>
{
    public RejectCreditRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");
    }
} 