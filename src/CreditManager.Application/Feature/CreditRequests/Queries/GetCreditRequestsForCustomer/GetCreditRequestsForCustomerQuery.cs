using CreditManager.Application.Common.Models;
using MediatR;

namespace CreditManager.Application.Feature.CreditRequests.Queries.GetCreditRequestsForCustomer;

public record GetCreditRequestsForCustomerQuery : IRequest<Result<IEnumerable<CreditRequestDto>>>;