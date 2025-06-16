using CreditManager.Application.Contracts.Persistence;
using CreditManager.Application.Messages;
using CreditManager.Domain.Entities.Credit;
using CreditManager.Infrastructure.Messaging;
using MassTransit;
using Moq;

namespace CreditManager.Infrastructure.Tests.Messaging;

public class CreditRequestMessageConsumerTests
{
    private readonly Mock<IAsyncRepository<Guid, SentCreditRequest>> _mockSendRequestRepository;
    private readonly Mock<IAsyncRepository<Guid, CreditRequest>> _mockCreditRequestRepository;
    private readonly CreditRequestMessageConsumer _consumer;
    private readonly CreditRequestSentMessage _validMessage;

    public CreditRequestMessageConsumerTests()
    {
        _mockSendRequestRepository = new Mock<IAsyncRepository<Guid, SentCreditRequest>>();
        _mockCreditRequestRepository = new Mock<IAsyncRepository<Guid, CreditRequest>>();
        _consumer = new CreditRequestMessageConsumer(_mockSendRequestRepository.Object, _mockCreditRequestRepository.Object);
        _validMessage = new CreditRequestSentMessage
        {
            Id = Guid.NewGuid(),
            CreditRequestId = Guid.NewGuid(),
            SendTime = DateTime.UtcNow
        };
    }

    [Fact]
    public async Task Consume_WhenSendRequestExists_ShouldNotCreateNew()
    {
        // Arrange
        var existingSendRequest = new SentCreditRequest
        {
            Id = _validMessage.Id,
            CreditRequestId = _validMessage.CreditRequestId,
            SendTime = _validMessage.SendTime
        };

        _mockSendRequestRepository
            .Setup(x => x.GetByIdAsync(_validMessage.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingSendRequest);

        var context = Mock.Of<ConsumeContext<CreditRequestSentMessage>>(x => 
            x.Message == _validMessage);

        // Act
        await _consumer.Consume(context);

        // Assert
        _mockSendRequestRepository.Verify(
            x => x.AddAsync(It.IsAny<SentCreditRequest>(), It.IsAny<CancellationToken>()),
            Times.Never);

        _mockCreditRequestRepository.Verify(
            x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Consume_WhenCreditRequestNotFound_ShouldNotCreateSendRequest()
    {
        // Arrange
        _mockSendRequestRepository
            .Setup(x => x.GetByIdAsync(_validMessage.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((SentCreditRequest)null);

        _mockCreditRequestRepository
            .Setup(x => x.GetByIdAsync(_validMessage.CreditRequestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((CreditRequest)null);

        var context = Mock.Of<ConsumeContext<CreditRequestSentMessage>>(x => 
            x.Message == _validMessage);

        // Act
        await _consumer.Consume(context);

        // Assert
        _mockSendRequestRepository.Verify(
            x => x.AddAsync(It.IsAny<SentCreditRequest>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Consume_WhenValid_ShouldCreateSendRequest()
    {
        // Arrange
        var creditRequest = new CreditRequest
        {
            Id = _validMessage.CreditRequestId,
            Status = CreditRequestStatus.Sent
        };

        _mockSendRequestRepository
            .Setup(x => x.GetByIdAsync(_validMessage.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((SentCreditRequest)null);

        _mockCreditRequestRepository
            .Setup(x => x.GetByIdAsync(_validMessage.CreditRequestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(creditRequest);

        _mockSendRequestRepository
            .Setup(x => x.AddAsync(It.IsAny<SentCreditRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((SentCreditRequest sr, CancellationToken ct) => sr);

        var context = Mock.Of<ConsumeContext<CreditRequestSentMessage>>(x => 
            x.Message == _validMessage);

        // Act
        await _consumer.Consume(context);

        // Assert
        _mockSendRequestRepository.Verify(
            x => x.AddAsync(It.Is<SentCreditRequest>(sr => 
                sr.Id == _validMessage.Id &&
                sr.CreditRequestId == _validMessage.CreditRequestId &&
                sr.SendTime == _validMessage.SendTime),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
} 