namespace CreditManager.Application.Pagination;

public interface IQueryObject
{
    public string? SortBy { get; set; }

    public string? SortDirection { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }
}
