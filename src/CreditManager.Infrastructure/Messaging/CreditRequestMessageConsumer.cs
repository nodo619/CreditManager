using CreditManager.Application.Contracts.Persistence;
using CreditManager.Application.Messages;
using CreditManager.Domain.Entities.Credit;
using MassTransit;

namespace CreditManager.Infrastructure.Messaging;

public class CreditRequestMessageConsumer : IConsumer<CreditRequestSentMessage>
{
    private readonly IAsyncRepository<Guid, SentCreditRequest> _sendRequestRepository;
    private readonly IAsyncRepository<Guid, CreditRequest> _creditRequestRepository;

    public CreditRequestMessageConsumer(
        IAsyncRepository<Guid, SentCreditRequest> sendRequestRepository,
        IAsyncRepository<Guid, CreditRequest> creditRequestRepository)
    {
        _sendRequestRepository = sendRequestRepository;
        _creditRequestRepository = creditRequestRepository;
    }

    public async Task Consume(ConsumeContext<CreditRequestSentMessage> context)
    {
        var message = context.Message;

        var existingSendRequest = await _sendRequestRepository.GetByIdAsync(message.Id, context.CancellationToken);

        if (existingSendRequest is {})
        {
            return;
        }

        var existingRequest = await _creditRequestRepository.GetByIdAsync(message.CreditRequestId, context.CancellationToken);

        if (existingRequest is null)
        {
            return;
        }

        var sentCreditRequest = new SentCreditRequest
        {
            Id = message.Id,
            CreditRequestId = message.CreditRequestId,
            SendTime = message.SendTime
        };

        await _sendRequestRepository.AddAsync(sentCreditRequest, context.CancellationToken);
    }
}