using CreditManager.Application.Common.Models;
using CreditManager.Application.Contracts.Persistence;
using CreditManager.Application.Feature.CreditRequests.Queries.GetCreditRequests;
using CreditManager.Application.Pagination;
using CreditManager.Domain.Entities.Credit;
using FluentAssertions;
using Moq;

namespace CreditManager.Application.Tests.Feature.CreditRequests.Queries.GetCreditRequests;

public class GetCreditRequestsQueryHandlerTests
{
    private readonly Mock<ICreditReadRepository> _mockRepository;
    private readonly GetCreditRequestsQueryHandler _handler;
    private readonly GetCreditRequestsQuery _validQuery;

    public GetCreditRequestsQueryHandlerTests()
    {
        _mockRepository = new Mock<ICreditReadRepository>();
        _handler = new GetCreditRequestsQueryHandler(_mockRepository.Object);
        _validQuery = new GetCreditRequestsQuery
        {
            PageNumber = 1,
            PageSize = 10,
            SortBy = "RequestDate",
            SortDirection = "desc"
        };
    }

    [Fact]
    public async Task Handle_WhenRequestsExist_ShouldReturnPaginatedList()
    {
        // Arrange
        var creditRequests = new List<CreditRequestWithUserModel>
        {
            new()
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                Amount = 1000,
                CurrencyCode = "USD",
                RequestDate = DateTime.UtcNow,
                Status = CreditRequestStatus.Sent,
                CustomerFirstName = "John",
                CustomerLastName = "Doe",
                CustomerUsername = "johndoe",
                CustomerPersonalNumber = "123456789"
            }
        };

        var paginatedList = new PaginatedList<CreditRequestWithUserModel>(creditRequests, 1);

        _mockRepository
            .Setup(x => x.GetCreditsWithSpecificStatusesAsync(
                It.IsAny<int[]>(),
                It.IsAny<IQueryObject>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedList);

        // Act
        var result = await _handler.Handle(_validQuery, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Items.Should().HaveCount(1);
        result.Value.TotalCount.Should().Be(1);

        var creditRequest = result.Value.Items.First();
        creditRequest.Id.Should().Be(creditRequests[0].Id);
        creditRequest.CustomerId.Should().Be(creditRequests[0].CustomerId);
        creditRequest.Amount.Should().Be(creditRequests[0].Amount);
        creditRequest.CurrencyCode.Should().Be(creditRequests[0].CurrencyCode);
        creditRequest.Status.Should().Be(creditRequests[0].Status);
        creditRequest.Customer.Should().NotBeNull();
        creditRequest.Customer!.FirstName.Should().Be(creditRequests[0].CustomerFirstName);
        creditRequest.Customer.LastName.Should().Be(creditRequests[0].CustomerLastName);
        creditRequest.Customer.Username.Should().Be(creditRequests[0].CustomerUsername);
        creditRequest.Customer.PersonalNumber.Should().Be(creditRequests[0].CustomerPersonalNumber);
    }

    [Fact]
    public async Task Handle_WhenNoRequestsExist_ShouldReturnEmptyList()
    {
        // Arrange
        var emptyList = new PaginatedList<CreditRequestWithUserModel>(new List<CreditRequestWithUserModel>(), 0);

        _mockRepository
            .Setup(x => x.GetCreditsWithSpecificStatusesAsync(
                It.IsAny<int[]>(),
                It.IsAny<IQueryObject>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyList);

        // Act
        var result = await _handler.Handle(_validQuery, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Items.Should().BeEmpty();
        result.Value.TotalCount.Should().Be(0);
    }
} 