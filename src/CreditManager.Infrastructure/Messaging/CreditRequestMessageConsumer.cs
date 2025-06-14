using CreditManager.Application.Contracts.Persistence;
using CreditManager.Application.Messages;
using CreditManager.Domain.Entities.Credit;
using MassTransit;

namespace CreditManager.Infrastructure.Messaging;

public class CreditRequestMessageConsumer : IConsumer<CreditRequestMessage>
{
    private readonly IAsyncRepository<Guid, CreditRequest> _repository;

    public CreditRequestMessageConsumer(IAsyncRepository<Guid, CreditRequest> repository)
    {
        _repository = repository;
    }
    
    public async Task Consume(ConsumeContext<CreditRequestMessage> context)
    {
        var message = context.Message;

        var existingRequest = await _repository.GetByIdAsync(message.Id, context.CancellationToken);

        if (existingRequest is { })
        {
            return;
        }
        
        var creditRequest = new CreditRequest
        {
            Id = message.Id,
            CustomerId = message.CustomerId,
            Amount = message.Amount,
            CurrencyCode = message.CurrencyCode,
            Status = CreditRequestStatus.Pending,
            Comments = message.Comments
        };
        
        //await _repository.AddAsync(creditRequest, context.CancellationToken);
    }
}