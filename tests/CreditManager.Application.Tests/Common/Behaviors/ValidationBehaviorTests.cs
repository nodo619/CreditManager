using CreditManager.Application.Common.Models;
using CreditManager.Application.Feature.CreditRequests.Commands.CreateCreditRequest;
using CreditManager.Application.PipelineBehaviors;
using CreditManager.Domain.Entities.Credit;
using Moq;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;

namespace CreditManager.Application.Tests.Common.Behaviors;

public class ValidationBehaviorTests
{
    private readonly Mock<IValidator<CreateCreditRequestCommand>> _mockValidator;
    private readonly ValidationBehavior<CreateCreditRequestCommand, Result<Guid>> _behavior;
    private readonly CreateCreditRequestCommand _validRequest;

    public ValidationBehaviorTests()
    {
        _mockValidator = new Mock<IValidator<CreateCreditRequestCommand>>();
        _behavior = new ValidationBehavior<CreateCreditRequestCommand, Result<Guid>>(new[] { _mockValidator.Object });
        
        _validRequest = new CreateCreditRequestCommand(
            Amount: 1000,
            CurrencyCode: "USD",
            PeriodYears: 1,
            PeriodMonths: 0,
            PeriodDays: 0,
            CreditType: (int)CreditType.QuickCredit,
            Comments: "Test credit request"
        );
    }

    [Fact]
    public async Task Handle_WhenRequestIsValid_ShouldCallNextAndReturnSuccess()
    {
        // Arrange
        _mockValidator
            .Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<CreateCreditRequestCommand>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var nextCalled = false;
        async Task<Result<Guid>> Next(CancellationToken cancellationToken)
        {
            nextCalled = true;
            return Result<Guid>.Success(Guid.NewGuid());
        }

        // Act
        var result = await _behavior.Handle(_validRequest, Next, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        nextCalled.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenRequestIsInvalid_ShouldThrowValidationException()
    {
        // Arrange
        var validationFailures = new List<ValidationFailure>
        {
            new("Amount", "Amount must be greater than 0")
        };

        _mockValidator
            .Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<CreateCreditRequestCommand>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(validationFailures));

        var nextCalled = false;
        async Task<Result<Guid>> Next(CancellationToken cancellationToken)
        {
            nextCalled = true;
            return Result<Guid>.Success(Guid.NewGuid());
        }

        // Act
        var act = () => _behavior.Handle(_validRequest, Next, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<ValidationException>()
            .Where(ex => ex.Errors.Any(e => e.ErrorMessage == "Amount must be greater than 0"));
        
        nextCalled.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_WhenValidatorThrowsException_ShouldPropagateException()
    {
        // Arrange
        _mockValidator
            .Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<CreateCreditRequestCommand>>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Validation failed"));

        async Task<Result<Guid>> Next(CancellationToken cancellationToken)
        {
            return Result<Guid>.Success(Guid.NewGuid());
        }

        // Act
        var act = () => _behavior.Handle(_validRequest, Next, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<Exception>()
            .WithMessage("Validation failed");
    }
} 