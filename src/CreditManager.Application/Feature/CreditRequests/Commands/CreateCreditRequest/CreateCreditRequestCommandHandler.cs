using CreditManager.Application.Contracts.Infrastructure;
using CreditManager.Application.Messages;
using MassTransit;
using MediatR;

namespace CreditManager.Application.Feature.CreditRequests.Commands.CreateCreditRequest;

public class CreateCreditRequestCommandHandler : IRequestHandler<CreateCreditRequestCommand, Guid>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ICurrentUserService _currentUserService;

    public CreateCreditRequestCommandHandler(IPublishEndpoint publishEndpoint, ICurrentUserService currentUserService)
    {
        _publishEndpoint = publishEndpoint;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(CreateCreditRequestCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _currentUserService.GetCurrentUserAsync(cancellationToken);

        if (currentUser is null)
        {
            throw new ArgumentNullException(nameof(currentUser));
        }

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
            CustomerId = currentUser.Id,
            RequestDate = DateTime.UtcNow
        };

        await _publishEndpoint.Publish(message);

        return message.Id;
    }
}