using MediatR;

namespace TaskManagementService.CommonLib.Requests
{
    public class PaginatedListRequest<TModel> : IRequest<PaginatedList<TModel>>
         where TModel : class
    {
        /// <summary>
        /// Номер запрашиваемой страницы
        /// </summary>
        public int PageNumber { get; init; } = 1;
        /// <summary>
        /// Размер страницы
        /// </summary>
        public int PageSize { get; init; } = 1000;
    }
}
