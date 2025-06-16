using CreditManager.Application.Common.Models;
using CreditManager.Application.Feature.CreditRequests.Commands.SendCreditRequest;
using CreditManager.Domain.Entities.Credit;
using Moq;
using FluentAssertions;
using CreditManager.Application.Contracts.Persistence;
using CreditManager.Application.Contracts.Infrastructure;
using CreditManager.Application.Messages;
using CreditManager.Domain.Entities.Identity;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace CreditManager.Application.Tests.Feature.CreditRequests.Commands.SendCreditRequest;

public class SendCreditRequestCommandHandlerTests
{
    private readonly Mock<IAsyncRepository<Guid, CreditRequest>> _mockCreditRepository;
    private readonly Mock<ICurrentUserService> _mockCurrentUserService;
    private readonly Mock<IPublishEndpoint> _mockPublishEndpoint;
    private readonly Mock<ILogger<SendCreditRequestCommandHandler>> _mockLogger;
    private readonly SendCreditRequestCommandHandler _handler;
    private readonly SendCreditRequestCommand _validCommand;
    private readonly User _currentUser;

    public SendCreditRequestCommandHandlerTests()
    {
        _mockCreditRepository = new Mock<IAsyncRepository<Guid, CreditRequest>>();
        _mockCurrentUserService = new Mock<ICurrentUserService>();
        _mockPublishEndpoint = new Mock<IPublishEndpoint>();
        _mockLogger = new Mock<ILogger<SendCreditRequestCommandHandler>>();
        
        _handler = new SendCreditRequestCommandHandler(
            _mockCreditRepository.Object,
            _mockCurrentUserService.Object,
            _mockPublishEndpoint.Object,
            _mockLogger.Object
        );
        
        _validCommand = new SendCreditRequestCommand(Guid.NewGuid());
        _currentUser = new User { Id = Guid.NewGuid() };
    }

    [Fact]
    public async Task Handle_WhenRequestExists_ShouldUpdateStatusToSent()
    {
        // Arrange
        var creditRequest = new CreditRequest
        {
            Id = _validCommand.Id,
            Status = CreditRequestStatus.Pending
        };

        _mockCurrentUserService
            .Setup(x => x.GetCurrentUserAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_currentUser);

        _mockCreditRepository
            .Setup(x => x.GetByIdAsync(_validCommand.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(creditRequest);

        _mockCreditRepository
            .Setup(x => x.UpdateAsync(It.IsAny<CreditRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockPublishEndpoint
            .Setup(x => x.Publish(It.IsAny<CreditRequestSentMessage>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(_validCommand, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        creditRequest.Status.Should().Be(CreditRequestStatus.Sent);
        creditRequest.LastModifiedBy.Should().Be(_currentUser.Id.ToString());
        
        _mockCreditRepository.Verify(
            x => x.UpdateAsync(It.Is<CreditRequest>(cr => 
                cr.Id == _validCommand.Id && 
                cr.Status == CreditRequestStatus.Sent), 
                It.IsAny<CancellationToken>()),
            Times.Once);

        _mockPublishEndpoint.Verify(
            x => x.Publish(It.Is<CreditRequestSentMessage>(m => 
                m.CreditRequestId == _validCommand.Id), 
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRequestNotFound_ShouldReturnFailure()
    {
        // Arrange
        _mockCreditRepository
            .Setup(x => x.GetByIdAsync(_validCommand.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((CreditRequest)null);

        // Act
        var result = await _handler.Handle(_validCommand, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Credit request not found");
        
        _mockCurrentUserService.Verify(
            x => x.GetCurrentUserAsync(It.IsAny<CancellationToken>()),
            Times.Never);

        _mockCreditRepository.Verify(
            x => x.UpdateAsync(It.IsAny<CreditRequest>(), It.IsAny<CancellationToken>()),
            Times.Never);

        _mockPublishEndpoint.Verify(
            x => x.Publish(It.IsAny<CreditRequestSentMessage>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenUserNotFound_ShouldReturnFailure()
    {
        // Arrange
        var creditRequest = new CreditRequest
        {
            Id = _validCommand.Id,
            Status = CreditRequestStatus.Pending
        };

        _mockCreditRepository
            .Setup(x => x.GetByIdAsync(_validCommand.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(creditRequest);

        _mockCurrentUserService
            .Setup(x => x.GetCurrentUserAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null);

        // Act
        var result = await _handler.Handle(_validCommand, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("User not found");
        
        _mockCreditRepository.Verify(
            x => x.UpdateAsync(It.IsAny<CreditRequest>(), It.IsAny<CancellationToken>()),
            Times.Never);

        _mockPublishEndpoint.Verify(
            x => x.Publish(It.IsAny<CreditRequestSentMessage>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenPublishFails_ShouldReturnFailure()
    {
        // Arrange
        var creditRequest = new CreditRequest
        {
            Id = _validCommand.Id,
            Status = CreditRequestStatus.Pending
        };

        _mockCurrentUserService
            .Setup(x => x.GetCurrentUserAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_currentUser);

        _mockCreditRepository
            .Setup(x => x.GetByIdAsync(_validCommand.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(creditRequest);

        _mockCreditRepository
            .Setup(x => x.UpdateAsync(It.IsAny<CreditRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockPublishEndpoint
            .Setup(x => x.Publish(It.IsAny<CreditRequestSentMessage>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Publish failed"));

        // Act
        var result = await _handler.Handle(_validCommand, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Error publishing a message");
    }
} 