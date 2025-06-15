using CreditManager.Application.Contracts.Persistence;
using MediatR;

namespace CreditManager.Application.Feature.CreditRequests.Queries.GetCreditRequest;

public class GetCreditRequestQueryHandler : IRequestHandler<GetCreditRequestQuery, CreditRequestDto>
{
    private readonly ICreditReadRepository _repository;

    public GetCreditRequestQueryHandler(ICreditReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<CreditRequestDto> Handle(GetCreditRequestQuery request, CancellationToken cancellationToken)
    {
        var creditRequest = await _repository.GetCreditByIdAsync(request.Id);

        if (creditRequest == null)
        {
            throw new KeyNotFoundException($"Credit request with ID {request.Id} was not found.");
        }

        return new CreditRequestDto
        {
            Id = creditRequest.Id,
            CustomerId = creditRequest.CustomerId,
            Amount = creditRequest.Amount,
            CurrencyCode = creditRequest.CurrencyCode,
            RequestDate = creditRequest.RequestDate,
            Period = creditRequest.Period,
            CreditType = creditRequest.CreditType,
            Status = creditRequest.Status,
            Comments = creditRequest.Comments,
            ApprovalDate = creditRequest.ApprovalDate,
            ApprovedBy = creditRequest.ApprovedBy
        };
    }
} 