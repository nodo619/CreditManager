using CreditManager.Application.Common.Models;
using CreditManager.Application.Contracts.Persistence;
using MediatR;

namespace CreditManager.Application.Feature.CreditRequests.Queries.GetCreditRequest;

public class GetCreditRequestQueryHandler : IRequestHandler<GetCreditRequestQuery, Result<CreditRequestDto>>
{
    private readonly ICreditReadRepository _repository;

    public GetCreditRequestQueryHandler(ICreditReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<CreditRequestDto>> Handle(GetCreditRequestQuery request, CancellationToken cancellationToken)
    {
        var creditRequest = await _repository.GetCreditByIdAsync(request.Id, cancellationToken);

        if (creditRequest == null)
        {
            return Result<CreditRequestDto>.Failure($"Credit request with ID {request.Id} was not found.");
        }

        return Result<CreditRequestDto>.Success(new CreditRequestDto
        {
            Id = creditRequest.Id,
            CustomerId = creditRequest.CustomerId,
            Amount = creditRequest.Amount,
            CurrencyCode = creditRequest.CurrencyCode,
            RequestDate = creditRequest.RequestDate,
            PeriodYears = creditRequest.PeriodYears,
            PeriodMonths = creditRequest.PeriodMonths,
            PeriodDays = creditRequest.PeriodDays,
            CreditType = creditRequest.CreditType,
            Status = creditRequest.Status,
            Comments = creditRequest.Comments,
            ApprovalDate = creditRequest.ApprovalDate,
            ApprovedBy = creditRequest.ApprovedBy
        });
    }
} 