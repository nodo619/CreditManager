using CreditManager.Application.Feature.CreditRequests.Commands.SendCreditRequest;
using FluentValidation;

namespace CreditManager.Application.Feature.CreditRequests.Commands.SendCreditRequest;

public class SendCreditRequestValidator : AbstractValidator<SendCreditRequestCommand>
{
    public SendCreditRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");
    }
} 