using MediatR;

namespace CreditManager.Application.Feature.CreditRequests.Queries.GetCreditRequests;

public record GetCreditRequestsQuery : IRequest<IEnumerable<CreditRequestDto>>; 