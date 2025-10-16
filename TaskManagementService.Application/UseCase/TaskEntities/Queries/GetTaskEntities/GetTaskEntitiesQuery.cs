using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using TaskManagementService.Application.Interfaces;
using TaskManagementService.Domain.Entities;
using TaskManagementService.CommonLib.Extentions;
using TaskManagementService.CommonLib.Requests;
using TaskManagementService.Application.UseCase.TaskEntities.Models;

namespace TaskManagementService.Application.UseCase.TaskEntities.Queries.GetTaskEntities
{
    public class GetTaskEntitiesQuery : PaginatedListRequest<TaskEntityModel>
    {
        public Guid[]? Ids { get; set; }
        public string? UserId { get; set; }
    }

    public class GetTaskEntitiesQueryHandler : IRequestHandler<GetTaskEntitiesQuery, PaginatedList<TaskEntityModel>>
    {
        private readonly IApplicationDbContext context;
        private readonly IMapper mapper;

        public GetTaskEntitiesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<PaginatedList<TaskEntityModel>> Handle(GetTaskEntitiesQuery query, CancellationToken cancellationToken)
        {
            IQueryable<TaskEntity> queue = context.TaskEntities;

            if (query.Ids?.Length > 0)
            {
                queue = queue.Where(x => query.Ids.Any(b => b == x.Id));
            }

            if (!string.IsNullOrWhiteSpace(query.UserId))
            {
                queue = queue.Where(x => x.UserId == query.UserId);
            }

            var taskEntity = await queue
                .OrderByDescending(x => x.CreatedAt)
                .ProjectTo<TaskEntityModel>(mapper.ConfigurationProvider)
                .PaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);

            return taskEntity;
        }
    }

}
