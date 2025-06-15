namespace CreditManager.Application.Pagination;

public class PaginatedList<T>
{
    public IReadOnlyCollection<T> Items { get; private set; }

    public int TotalCount { get; private set; }


    public PaginatedList(IReadOnlyCollection<T> items, int count)
    {
        TotalCount = count;
        Items = items;
    }
}
