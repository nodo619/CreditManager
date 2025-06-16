using CreditManager.Application.Common.Models;
using MediatR;

namespace CreditManager.Application.Feature.CreditRequests.Commands.SendCreditRequest;

public record SendCreditRequestCommand(Guid Id) : IRequest<Result<Unit>>; 