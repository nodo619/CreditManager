using CreditManager.Application.Feature.CreditRequests.Commands.CreateCreditRequest;
using CreditManager.Application.Contracts.Persistence;
using CreditManager.Application.Contracts.Infrastructure;
using CreditManager.Domain.Entities.Credit;
using CreditManager.Domain.Entities.Identity;
using Moq;
using FluentAssertions;

namespace CreditManager.Application.Tests.Feature.CreditRequests.Commands.CreateCreditRequest;

public class CreateCreditRequestCommandHandlerTests
{
    private readonly Mock<IAsyncRepository<Guid, CreditRequest>> _mockRepository;
    private readonly Mock<ICurrentUserService> _mockCurrentUserService;
    private readonly CreateCreditRequestCommandHandler _handler;

    public CreateCreditRequestCommandHandlerTests()
    {
        _mockRepository = new Mock<IAsyncRepository<Guid, CreditRequest>>();
        _mockCurrentUserService = new Mock<ICurrentUserService>();
        _handler = new CreateCreditRequestCommandHandler(_mockCurrentUserService.Object, _mockRepository.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldCreateCreditRequest()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new CreateCreditRequestCommand(
            Amount: 1000,
            CurrencyCode: "USD",
            PeriodYears: 1,
            PeriodMonths: 0,
            PeriodDays: 0,
            CreditType: (int)CreditType.QuickCredit,
            Comments: "Test credit request"
        );

        _mockCurrentUserService
            .Setup(x => x.GetCurrentUserAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User { Id = userId });

        _mockRepository
            .Setup(x => x.AddAsync(It.IsAny<CreditRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((CreditRequest cr, CancellationToken ct) => cr);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _mockRepository.Verify(x => x.AddAsync(It.Is<CreditRequest>(cr => 
            cr.CustomerId == userId &&
            cr.Amount == command.Amount &&
            cr.CurrencyCode == command.CurrencyCode &&
            cr.CreditType == (CreditType)command.CreditType &&
            cr.PeriodYears == command.PeriodYears &&
            cr.PeriodMonths == command.PeriodMonths &&
            cr.PeriodDays == command.PeriodDays &&
            cr.Comments == command.Comments &&
            cr.Status == CreditRequestStatus.Pending
        ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_UserNotFound_ShouldReturnFailure()
    {
        // Arrange
        var command = new CreateCreditRequestCommand(
            Amount: 1000,
            CurrencyCode: "USD",
            PeriodYears: 1,
            PeriodMonths: 0,
            PeriodDays: 0,
            CreditType: (int)CreditType.QuickCredit,
            Comments: "Test credit request"
        );

        _mockCurrentUserService
            .Setup(x => x.GetCurrentUserAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Current user not found");
        _mockRepository.Verify(x => x.AddAsync(It.IsAny<CreditRequest>(), It.IsAny<CancellationToken>()), Times.Never);
    }
} 