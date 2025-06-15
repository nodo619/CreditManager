using CreditManager.Application.Common.Models;
using MediatR;

namespace CreditManager.Application.Feature.CreditRequests.Commands.CancelCreditRequest;

public record CancelCreditRequestCommand(Guid Id) : IRequest<Result<Unit>>; 