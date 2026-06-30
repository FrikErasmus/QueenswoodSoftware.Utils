namespace Library.Helpers;

public class PagedResult<T>
{
    public List<T> Items { get; set; } = [];
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;
}

public static class PaginationHelper
{
    public static PagedResult<T> Paginate<T>(IEnumerable<T> source, int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        var list = source.ToList();
        return new PagedResult<T>
        {
            TotalCount = list.Count,
            Page = page,
            PageSize = pageSize,
            Items = list.Skip((page - 1) * pageSize).Take(pageSize).ToList()
        };
    }

    public static int TotalPages(int totalCount, int pageSize) =>
        pageSize == 0 ? 0 : (int)Math.Ceiling(totalCount / (double)pageSize);

    public static bool IsValidPage(int page, int totalPages) =>
        page >= 1 && page <= totalPages;
}
