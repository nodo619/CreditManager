using MediatR;

namespace CreditManager.Application.Feature.CreditRequests.Queries.GetCreditRequest;

public record GetCreditRequestQuery(Guid Id) : IRequest<CreditRequestDto>; 