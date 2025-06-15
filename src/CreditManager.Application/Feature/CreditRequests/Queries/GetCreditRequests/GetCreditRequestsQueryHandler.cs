using CreditManager.Application.Feature.CreditRequests.Queries.GetCreditRequest;
using MediatR;

namespace CreditManager.Application.Feature.CreditRequests.Queries.GetCreditRequests;

public class GetCreditRequestsQueryHandler : IRequestHandler<GetCreditRequestsQuery, IEnumerable<CreditRequestDto>>
{
    // private readonly IApplicationDbContext _context;
    //
    // public GetCreditRequestsQueryHandler(IApplicationDbContext context)
    // {
    //     _context = context;
    // }

    public async Task<IEnumerable<CreditRequestDto>> Handle(GetCreditRequestsQuery request, CancellationToken cancellationToken)
    {
        // return await _context.CreditRequests
        //     .Select(cr => new CreditRequestDto
        //     {
        //         Id = cr.Id,
        //         CustomerId = cr.CustomerId,
        //         Amount = cr.Amount,
        //         CurrencyCode = cr.CurrencyCode,
        //         RequestDate = cr.RequestDate,
        //         Period = cr.Period,
        //         CreditType = cr.CreditType,
        //         Status = cr.Status,
        //         Comments = cr.Comments,
        //         ApprovalDate = cr.ApprovalDate,
        //         ApprovedBy = cr.ApprovedBy,
        //         CreatedAt = cr.CreatedAt,
        //         LastModifiedAt = cr.LastModifiedAt
        //     })
        //     .ToListAsync(cancellationToken);
        return null;
    }
} 