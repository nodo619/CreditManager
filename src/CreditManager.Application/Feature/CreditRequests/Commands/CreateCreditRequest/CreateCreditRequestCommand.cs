using CreditManager.Application.Common.Models;
using MediatR;

namespace CreditManager.Application.Feature.CreditRequests.Commands.CreateCreditRequest;

public record CreateCreditRequestCommand(
    decimal Amount,
    string CurrencyCode,
    string? Comments,
    int CreditType,
    int PeriodYears,
    int PeriodMonths,
    int PeriodDays
    ) : IRequest<Result<Guid>>;