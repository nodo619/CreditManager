using CreditManager.Application.Common.Models;
using MediatR;

namespace CreditManager.Application.Feature.CreditRequests.Commands.RejectCreditRequest;

public record RejectCreditRequestCommand(Guid Id) : IRequest<Result<Unit>>; 