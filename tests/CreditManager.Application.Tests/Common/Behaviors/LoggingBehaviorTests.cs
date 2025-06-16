using CreditManager.Application.Common.Models;
using CreditManager.Application.Feature.CreditRequests.Commands.SendCreditRequest;
using CreditManager.Application.PipelineBehaviors;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace CreditManager.Application.Tests.Common.Behaviors;

public class LoggingBehaviorTests
{
    private readonly Mock<ILogger<LoggingBehavior<SendCreditRequestCommand, Result<Unit>>>> _mockLogger;
    private readonly LoggingBehavior<SendCreditRequestCommand, Result<Unit>> _behavior;
    private readonly SendCreditRequestCommand _validRequest;

    public LoggingBehaviorTests()
    {
        _mockLogger = new Mock<ILogger<LoggingBehavior<SendCreditRequestCommand, Result<Unit>>>>();
        _behavior = new LoggingBehavior<SendCreditRequestCommand, Result<Unit>>(_mockLogger.Object);
        _validRequest = new SendCreditRequestCommand(Guid.NewGuid());
    }

    [Fact]
    public async Task Handle_WhenRequestIsValid_ShouldLogAndCallNext()
    {
        // Arrange
        var expectedResult = Result<Unit>.Success(Unit.Value);
        async Task<Result<Unit>> Next(CancellationToken cancellationToken)
        {
            return expectedResult;
        }

        // Act
        var result = await _behavior.Handle(_validRequest, Next, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(expectedResult);
        result.IsSuccess.Should().BeTrue();

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Handling")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Handled")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
} 