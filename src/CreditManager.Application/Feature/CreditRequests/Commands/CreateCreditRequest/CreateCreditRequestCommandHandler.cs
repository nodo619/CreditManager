using CreditManager.Application.Common.Models;
using CreditManager.Application.Contracts.Infrastructure;
using CreditManager.Application.Messages;
using MassTransit;
using MediatR;

namespace CreditManager.Application.Feature.CreditRequests.Commands.CreateCreditRequest;

public class CreateCreditRequestCommandHandler : IRequestHandler<CreateCreditRequestCommand, Result<Guid>>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ICurrentUserService _currentUserService;

    public CreateCreditRequestCommandHandler(IPublishEndpoint publishEndpoint, ICurrentUserService currentUserService)
    {
        _publishEndpoint = publishEndpoint;
        _currentUserService = currentUserService;
    }

    public async Task<Result<Guid>> Handle(CreateCreditRequestCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _currentUserService.GetCurrentUserAsync(cancellationToken);

        if (currentUser is null)
        {
            return Result<Guid>.Failure("Current user not found");
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

        return Result<Guid>.Success(message.Id);
    }
}