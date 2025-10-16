using Microsoft.EntityFrameworkCore;
using TaskManagementService.CommonLib.Helpers;

namespace TaskManagementService.CommonLib.Requests
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; }
        public int PageNumber { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }
        public string OrderBy { get; set; }

        public PaginatedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            Items = items;
        }

        public PaginatedList(List<T> items, int count, int pageNumber, int pageSize, Enum orderBy) : this(items, count, pageNumber, pageSize)
        {
            if (orderBy == null) return;

            OrderBy = orderBy.GetOrdering();
        }

        public PaginatedList(List<T> items, int totalPages, int totalCount, int pageNumber, bool hasValue = true)
        {
            PageNumber = pageNumber;
            TotalPages = totalPages;
            TotalCount = totalCount;
            Items = items;
        }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;

        public bool HasItems => Items != null && Items.Count > 0;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var count = await source.CountAsync(cancellationToken);
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return new PaginatedList<T>(items, count, pageNumber, pageSize);
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, CancellationToken cancellationToken)
        {
            var pageNumber = 1;
            var count = await source.CountAsync(cancellationToken);

            var pageSize = count;
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return new PaginatedList<T>(items, count, pageNumber, pageSize);
        }

        public static Task<PaginatedList<T>> CreateAsync(List<T> list, CancellationToken cancellationToken)
        {
            return CreateAsync(list.AsQueryable(), cancellationToken);
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize, string orderBy, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                return await CreateAsync(source, pageNumber, pageSize, cancellationToken);

            var count = await source.CountAsync(cancellationToken);
            var items = await source.ApplyOrder(orderBy).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return new PaginatedList<T>(items, count, pageNumber, pageSize) { OrderBy = orderBy };
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize, Enum orderBy, CancellationToken cancellationToken)
        {
            if (orderBy == null)
                return await CreateAsync(source, pageNumber, pageSize, cancellationToken);

            var ordering = orderBy.GetOrdering();
            var count = await source.CountAsync(cancellationToken);
            var items = await source.ApplyOrder(ordering).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return new PaginatedList<T>(items, count, pageNumber, pageSize) { OrderBy = ordering };
        }

        public static PaginatedList<T> Create(List<T> list)
        {
            var pageNumber = 1;
            var count = list.Count();

            var pageSize = count;
            var items = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PaginatedList<T>(items, count, pageNumber, pageSize);
        }

        public static PaginatedList<T> Create(IEnumerable<T> list, int pageNumber, int pageSize)
        {
            var count = list.Count();

            var items = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PaginatedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
