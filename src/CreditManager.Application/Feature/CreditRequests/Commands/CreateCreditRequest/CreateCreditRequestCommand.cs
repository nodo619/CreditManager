using System.Windows.Input;
using MediatR;

namespace CreditManager.Application.Feature.CreditRequests.Commands.CreateCreditRequest;

public record CreateCreditRequestCommand(
    Guid CustomerId,
    decimal Amount,
    string CurrencyCode,
    string? Comments,
    int CreditType,
    int PeriodYears,
    int PeriodMonths,
    int PeriodDays
    ) : IRequest<Guid>;