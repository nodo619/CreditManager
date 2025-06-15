using MediatR;

namespace CreditManager.Application.Feature.CreditRequests.Queries.GetCreditRequestsForCustomer;

public record GetCreditRequestsForCustomerQuery : IRequest<IEnumerable<CreditRequestDto>>;