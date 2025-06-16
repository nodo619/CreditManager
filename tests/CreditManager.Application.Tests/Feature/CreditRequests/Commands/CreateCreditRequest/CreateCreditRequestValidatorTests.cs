using CreditManager.Application.Feature.CreditRequests.Commands.CreateCreditRequest;
using CreditManager.Domain.Entities.Credit;
using FluentAssertions;

namespace CreditManager.Application.Tests.Feature.CreditRequests.Commands.CreateCreditRequest;

public class CreateCreditRequestValidatorTests
{
    private readonly CreateCreditRequestValidator _validator;

    public CreateCreditRequestValidatorTests()
    {
        _validator = new CreateCreditRequestValidator();
    }

    [Fact]
    public void Validate_ValidRequest_ShouldPass()
    {
        // Arrange
        var request = new CreateCreditRequestCommand(
            Amount: 1000,
            CurrencyCode: "USD",
            PeriodYears: 1,
            PeriodMonths: 0,
            PeriodDays: 0,
            CreditType: (int)CreditType.QuickCredit,
            Comments: "Test credit request"
        );

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1000)]
    public void Validate_InvalidAmount_ShouldFail(decimal amount)
    {
        // Arrange
        var request = new CreateCreditRequestCommand(
            Amount: amount,
            CurrencyCode: "USD",
            PeriodYears: 1,
            PeriodMonths: 0,
            PeriodDays: 0,
            CreditType: (int)CreditType.QuickCredit,
            Comments: "Test credit request"
        );

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Amount");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("INVALID")]
    public void Validate_InvalidCurrencyCode_ShouldFail(string currencyCode)
    {
        // Arrange
        var request = new CreateCreditRequestCommand(
            Amount: 1000,
            CurrencyCode: currencyCode,
            PeriodYears: 1,
            PeriodMonths: 0,
            PeriodDays: 0,
            CreditType: (int)CreditType.QuickCredit,
            Comments: "Test credit request"
        );

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "CurrencyCode");
    }

    [Fact]
    public void Validate_InvalidPeriod_ShouldFail()
    {
        // Arrange
        var request = new CreateCreditRequestCommand(
            Amount: 1000,
            CurrencyCode: "USD",
            PeriodYears: 0,
            PeriodMonths: 0,
            PeriodDays: 0,
            CreditType: (int)CreditType.QuickCredit,
            Comments: "Test credit request"
        );

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage.Contains("Period can not be empty"));
    }

    [Fact]
    public void Validate_InvalidCreditType_ShouldFail()
    {
        // Arrange
        var request = new CreateCreditRequestCommand(
            Amount: 1000,
            CurrencyCode: "USD",
            PeriodYears: 1,
            PeriodMonths: 0,
            PeriodDays: 0,
            CreditType: 999, // Invalid enum value
            Comments: "Test credit request"
        );

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "CreditType");
    }
} 