using CreditManager.Application.Common.Models;
using CreditManager.Application.Contracts.Infrastructure;
using CreditManager.Application.Contracts.Persistence;
using CreditManager.Application.Messages;
using CreditManager.Domain.Entities.Credit;
using MassTransit;
using MediatR;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace CreditManager.Application.Feature.CreditRequests.Commands.SendCreditRequest;

public class SendCreditRequestCommandHandler : IRequestHandler<SendCreditRequestCommand, Result<Unit>>
{
    private readonly IAsyncRepository<Guid, CreditRequest> _repository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<SendCreditRequestCommandHandler> _logger;

    public SendCreditRequestCommandHandler(IAsyncRepository<Guid,
        CreditRequest> repository,
        ICurrentUserService currentUserService,
        IPublishEndpoint publishEndpoint,
        ILogger<SendCreditRequestCommandHandler> logger)
    {
        _repository = repository;
        _currentUserService = currentUserService;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(SendCreditRequestCommand request, CancellationToken cancellationToken)
    {
        var creditRequest = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (creditRequest == null)
        {
            return Result<Unit>.Failure("Credit request not found");
        }

        var currentUser = await _currentUserService.GetCurrentUserAsync(cancellationToken);

        if (currentUser is null)
        {
            return Result<Unit>.Failure("User not found");
        }

        creditRequest.Status = CreditRequestStatus.Sent;
        creditRequest.LastModifiedBy = currentUser.Id.ToString();

        await _repository.UpdateAsync(creditRequest, cancellationToken);

        var messageResult = await PublishSendMessage(request.Id, DateTime.UtcNow, cancellationToken);

        if (!messageResult.IsSuccess)
        {
            return messageResult;
        }

        return Result<Unit>.Success(Unit.Value);
    }

    private async Task<Result<Unit>> PublishSendMessage(Guid id, DateTime time, CancellationToken cancellationToken)
    {
        try
        {
            var message = new CreditRequestSentMessage
            {
                Id = Guid.NewGuid(),
                CreditRequestId = id,
                SendTime = time,
            };

            await _publishEndpoint.Publish(message, cancellationToken);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error publishing a message");

            //not sending exception message for security purposes
            return Result<Unit>.Failure("Error publishing a message");
        }

        return Result<Unit>.Success(Unit.Value);
    }
} 