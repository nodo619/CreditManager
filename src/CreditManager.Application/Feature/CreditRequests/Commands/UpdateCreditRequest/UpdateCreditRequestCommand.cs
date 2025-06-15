using CreditManager.Application.Common.Models;
using MediatR;

namespace CreditManager.Application.Feature.CreditRequests.Commands.UpdateCreditRequest;

public record UpdateCreditRequestCommand(
    Guid Id,
    decimal Amount,
    string CurrencyCode,
    string? Comments,
    int CreditType,
    int PeriodYears,
    int PeriodMonths,
    int PeriodDays
    ) : IRequest<Result<Unit>>;