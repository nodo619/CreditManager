using System.Windows.Input;
using MediatR;

namespace CreditManager.Application.Feature.CreditRequests.Commands.CreateCreditRequest;

public record CreateCreditRequestCommand(
    Guid CustomerId,
    decimal Amount,
    string CurrencyCode,
    string? Comments,
    int CreditType,
    TimeSpan Period
    ) : IRequest<Guid>;