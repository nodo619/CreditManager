using CreditManager.Application.Common.Models;
using MediatR;

namespace CreditManager.Application.Feature.CreditRequests.Commands.ApproveCreditRequest;

public record ApproveCreditRequestCommand(Guid Id) : IRequest<Result<Unit>>; 