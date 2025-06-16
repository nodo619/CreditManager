using CreditManager.Application.Feature.CreditRequests.Commands.SendCreditRequest;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace CreditManager.Application.Tests.Feature.CreditRequests.Commands.SendCreditRequest;

public class SendCreditRequestValidatorTests
{
    private readonly SendCreditRequestValidator _validator;

    public SendCreditRequestValidatorTests()
    {
        _validator = new SendCreditRequestValidator();
    }

    [Fact]
    public void Validate_WhenIdIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new SendCreditRequestCommand(Guid.Empty);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Id is required");
    }

    [Fact]
    public void Validate_WhenIdIsValid_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new SendCreditRequestCommand(Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
} 