using FluentValidation;

namespace CreditManager.Application.Feature.CreditRequests.Commands.ApproveCreditRequest;

public class ApproveCreditRequestValidator : AbstractValidator<ApproveCreditRequestCommand>
{
    public ApproveCreditRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");
    }
} 