using CreditManager.Application.Contracts.Infrastructure;
using CreditManager.Application.Messages;
using MassTransit;
using MediatR;

namespace CreditManager.Application.Feature.CreditRequests.Commands.CreateCreditRequest;

public class CreateCreditRequestCommandHandler : IRequestHandler<CreateCreditRequestCommand, Guid>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateCreditRequestCommandHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Guid> Handle(CreateCreditRequestCommand request, CancellationToken cancellationToken)
    {
        var message = new CreditRequestMessage
        {
            Id = Guid.NewGuid(),
            Amount = request.Amount,
            Comments = request.Comments,
            PeriodYears = request.PeriodYears,
            PeriodMonths = request.PeriodMonths,
            PeriodDays = request.PeriodDays,
            CreditType = request.CreditType,
            CurrencyCode = request.CurrencyCode,
            CustomerId = request.CustomerId,
            RequestDate = DateTime.Now
        };

        await _publishEndpoint.Publish(message);

        return message.Id;
    }
}