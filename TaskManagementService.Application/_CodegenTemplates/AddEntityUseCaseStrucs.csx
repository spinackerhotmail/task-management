using System.IO;
using System.Text;
using System.Reflection;

public class AddEntity
{
    void Main(ICodegenContext context)
    {
        #region Generate Commands 
        // TODO: заменить название на нужную сущность
        // ВАЖНО: после генерации вернуть на Test - если не вернуть, может включиться повторно генерация и создаст по новой файлы (затрет изменения)


        var entity = "TaskEntity";
        var entityPlural = "TaskEntities";
        var entityLower = "taskEntity";

        var projectName = "TaskManagementService.";

        var basePath = $"../UseCase/{entityPlural}";
        var commandPath = $"{basePath}/Commands";

        var entityCsFile = $"{entity}Command.cs";
        var createFile = $"Create{entityCsFile}";
        var updateFile = $"Update{entityCsFile}";
        var deleteFile = $"Delete{entityCsFile}";

        if (!File.Exists(createFile))
        {
            var contextCreate = new CodegenContext();

            contextCreate[createFile]
                .WriteLine(GenerateCreateCommand(projectName, entity, entityPlural, entityLower));

            contextCreate.SaveFiles(outputFolder: $"{commandPath}/Create{entity}");
        }

        if (!File.Exists(updateFile))
        {
            var contextUpdate = new CodegenContext();
            contextUpdate[updateFile]
                .WriteLine(GenerateUpdateCommand(projectName, entity, entityPlural, entityLower));

            contextUpdate.SaveFiles(outputFolder: $"{commandPath}/Update{entity}");
        }

        if (!File.Exists(deleteFile))
        {
            var contextDelete = new CodegenContext();
            contextDelete[deleteFile]
                .WriteLine(GenerateDeleteCommand(projectName, entity, entityPlural, entityLower));

            contextDelete.SaveFiles(outputFolder: $"{commandPath}/Delete{entity}");
        }
        #endregion

        #region Generate Validation Files
        var entityValidationFile = $"{entity}Validator.cs";
        var createValidationFile = $"Create{entityValidationFile}";
        var updateValidationFile = $"Update{entityValidationFile}";
        var deleteValidationFile = $"Delete{entityValidationFile}";

        if (!File.Exists(createValidationFile))
        {
            var contextValidation1 = new CodegenContext();

            contextValidation1[createValidationFile]
                .WriteLine(GenerateCreateValidator(projectName, entity, entityPlural, entityLower));

            contextValidation1.SaveFiles(outputFolder: $"{commandPath}/Create{entity}");
        }


        if (!File.Exists(updateValidationFile))
        {
            var contextValidation2 = new CodegenContext();
            contextValidation2[updateValidationFile]
                .WriteLine(GenerateUpdateValidator(projectName, entity, entityPlural, entityLower));

            contextValidation2.SaveFiles(outputFolder: $"{commandPath}/Update{entity}");
        }

        if (!File.Exists(deleteValidationFile))
        {
            var contextValidation3 = new CodegenContext();
            contextValidation3[deleteValidationFile]
                .WriteLine(GenerateDeleteValidator(projectName, entity, entityPlural, entityLower));

            contextValidation3.SaveFiles(outputFolder: $"{commandPath}/Delete{entity}");
        }


        #endregion

        #region Generate Models
        var modelsPath = $"{basePath}/Models";
        var response = $"{entity}Model.cs";


        var contextModels = new CodegenContext();

        if (!File.Exists(response))
        {
            contextModels[response]
                .WriteLine(GenerateModels(projectName, entity, entityPlural, entityLower));

            contextModels.SaveFiles(outputFolder: $"{modelsPath}");
        }
        #endregion

        #region Generate Queries
        var queriesPath = $"{basePath}/Queries/Get{entityPlural}";
        var query = $"Get{entityPlural}Query.cs";


        var contextQueries = new CodegenContext();

        if (!File.Exists(query))
        {
            contextQueries[query]
                .WriteLine(GenerateGetQuery(projectName, entity, entityPlural, entityLower));

            contextQueries.SaveFiles(outputFolder: $"{queriesPath}");
        }
        #endregion

        #region Generate Queries validator
        queriesPath = $"{basePath}/Queries/Get{entityPlural}";
        query = $"Get{entityPlural}QueryValidator.cs";


        var contextQueriesValidator = new CodegenContext();

        if (!File.Exists(query))
        {
            contextQueriesValidator[query]
                .WriteLine(GenerateGetQueryValidator(projectName, entity, entityPlural));

            contextQueriesValidator.SaveFiles(outputFolder: $"{queriesPath}");
        }
        #endregion

        #region Generate Mapping profile

        var mappingBasePath = $"../Common/Mapping";

        var mapping = $"{entity}Profile.cs";


        var mappingModels = new CodegenContext();

        if (!File.Exists(mapping))
        {
            mappingModels[mapping]
                .WriteLine(GenerateMapping(projectName, entity, entityPlural, entityLower));

            mappingModels.SaveFiles(outputFolder: $"{mappingBasePath}");
        }
        #endregion

        #region Genereate ResultConsumer Events
        //var resultBasePath = $"../ResultConsumer/{entityPlural}";
        //var resulEventPath = $"{resultBasePath}/Events";

        //var resultBaseEventFile = $"Base{entity}Event.cs";
        //var resultCreateFile = $"{entity}CreatedEvent.cs";
        //var resultUpdateFile = $"{entity}UpdatedEvent.cs";
        //var resultDeleteFile = $"{entity}DeletedEvent.cs";


        //if (!File.Exists(resultBaseEventFile))
        //{
        //    var contextCreated = new CodegenContext();
        //    contextCreated[resultBaseEventFile]
        //        .WriteLine(GenerateBaseEntityEvent(projectName, entity, entityPlural, entityLower));

        //    contextCreated.SaveFiles(outputFolder: $"{resulEventPath}/BaseEvents");
        //}

        //if (!File.Exists(resultCreateFile))
        //{
        //    var contextCreated = new CodegenContext();
        //    contextCreated[resultCreateFile]
        //        .WriteLine(GenerateEntityCreatedEvent(projectName, entity, entityPlural, entityLower));

        //    contextCreated.SaveFiles(outputFolder: $"{resulEventPath}/{entity}Created");
        //}


        //if (!File.Exists(resultUpdateFile))
        //{
        //    var context2 = new CodegenContext();
        //    context2[resultUpdateFile]
        //        .WriteLine(GenerateEntityUpdatedEvent(projectName, entity, entityPlural, entityLower));

        //    context2.SaveFiles(outputFolder: $"{resulEventPath}/{entity}Updated");
        //}

        //if (!File.Exists(resultDeleteFile))
        //{
        //    var context3 = new CodegenContext();
        //    context3[resultDeleteFile]
        //        .WriteLine(GenerateEntityDeletedEvent(projectName, entity, entityPlural, entityLower));

        //    context3.SaveFiles(outputFolder: $"{resulEventPath}/{entity}Deleted");
        //}
        #endregion

        #region Generate ResultConsumer Queries
        //var resultQueriesPath = $"{resultBasePath}/Queries/Get{entityPlural}";
        //var resultQuery = $"Get{entityPlural}Query.cs";



        //if (!File.Exists(resultQuery))
        //{
        //    var contextQueries2 = new CodegenContext();
        //    contextQueries2[resultQuery]
        //        .WriteLine(GenerateResultConsumerGetQuery(projectName, entity, entityPlural, entityLower));

        //    contextQueries2.SaveFiles(outputFolder: $"{resultQueriesPath}");

        //}
        #endregion

        #region Generate Configuration

        //var confPath = $"../UseCase/{entityPlural}/ConfigureService";
        //var confFile = "ConfigureServices.cs";

        //var confFilePath = $"{confPath}/{confFile}";

        //if (!File.Exists(confFilePath))
        //{
        //    var contextConf = new CodegenContext();
        //    contextConf[confFile]
        //        .WriteLine(GenerateConfigurationService(projectName, entity, entityPlural, entityLower));

        //    contextConf.SaveFiles(outputFolder: $"{confPath}");
        //}

        #endregion
    }

    FormattableString GenerateCreateCommand(string projectName, string entity, string entityPlural, string entityLower)
    {
        return $$"""
            using MediatR;
            using AutoMapper;
            using {{projectName}}Application.UseCase.Models.{{entityPlural}};
            using {{projectName}}Application.Interfaces;
            using {{projectName}}Domain.Entities;

            namespace {{projectName}}Application.UseCase.{{entityPlural}}.Commands.Create{{entity}}
            {
                public class Create{{entity}}Command : IRequest<{{entity}}Model>
                {
                    public string Name { get; set; }
                }

                public class Create{{entity}}CommandHandler : IRequestHandler<Create{{entity}}Command, {{entity}}Model>
                {
                    private readonly IMapper mapper;
                    private readonly IApplicationDbContext context;

                    public Create{{entity}}CommandHandler(IMapper mapper, IApplicationDbContext context)
                    {
                        this.mapper = mapper;
                        this.context = context;
                    }

                    public async Task<{{entity}}Model> Handle(Create{{entity}}Command command, CancellationToken cancellationToken)
                    {
                        var {{entityLower}} = mapper.Map<{{entity}}>(command);
                        context.{{entityPlural}}.Add({{entityLower}});

                        await context.SaveChangesAsync(cancellationToken);

                        return mapper.Map<{{entity}}Model>({{entityLower}});
                    }
                }
            }
            """;
    }

    FormattableString GenerateUpdateCommand(string projectName, string entity, string entityPlural, string entityLower)
    {
        return $$"""
            using MediatR;
            using AutoMapper;
            using Microsoft.EntityFrameworkCore;
            using {{projectName}}Application.Common.Models.{{entityPlural}};
            using {{projectName}}Application.Interfaces;
            using TaskManagementService.CommonLib.Exceptions;

            namespace {{projectName}}Application.UseCase.{{entityPlural}}.Commands.Update{{entity}}
            {
                public class Update{{entity}}Command : IRequest<{{entity}}Model>
                {
                    public long Id { get; set; }
                    public string Name { get; set; }
                }

                public class Update{{entity}}CommandHandler : IRequestHandler<Update{{entity}}Command, {{entity}}Model>
                {
                    private readonly IMapper mapper;
                    private readonly IApplicationDbContext context;

                    public Update{{entity}}CommandHandler(IMapper mapper, IApplicationDbContext context)
                    {
                        this.mapper = mapper;
                        this.context = context;
                    }

                    public async Task<{{entity}}Model> Handle(Update{{entity}}Command command, CancellationToken cancellationToken)
                    {
                        var {{entityLower}} = await context.{{entityPlural}}.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken)
                            ?? throw new NotFoundException($"Не найдено с Id = {command.Id}");

                        {{entityLower}}.Name = command.Name;

                        await context.SaveChangesAsync(cancellationToken);

                        return mapper.Map<{{entity}}Model>({{entityLower}});
                    }
                }

            }
            """;
    }

    FormattableString GenerateDeleteCommand(string projectName, string entity, string entityPlural, string entityLower)
    {
        return $$"""
                using MediatR;
                using AutoMapper;
                using Microsoft.EntityFrameworkCore;
                using {{projectName}}Application.Common.Models.{{entityPlural}};
                using {{projectName}}Application.Interfaces;
                using TaskManagementService.CommonLib.Exceptions;

                namespace {{projectName}}Application.UseCase.{{entityPlural}}.Commands.Delete{{entity}}
                {
                    public class Delete{{entity}}Command : IRequest<{{entity}}Model>
                    {
                        public long Id { get; set; }
                    }
                    
                    public class Delete{{entity}}CommandHandler : IRequestHandler<Delete{{entity}}Command, {{entity}}Model>
                    {
                        private readonly IApplicationDbContext context;
                        private readonly IMapper mapper;

                        public Delete{{entity}}CommandHandler(IApplicationDbContext context, IMapper mapper)
                        {
                            this.mapper = mapper;
                            this.context = context;
                        }

                        public async Task<{{entity}}Model> Handle(Delete{{entity}}Command command, CancellationToken cancellationToken)
                        {
                            var {{entityLower}} = await context.{{entityPlural}}.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken)
                                    ?? throw new NotFoundException($"Не найдено с Id = {command.Id}");

                            var result{{entity}} = mapper.Map<{{entity}}Model>({{entityLower}});

                            context.{{entityPlural}}.Remove({{entityLower}});
                            await context.SaveChangesAsync(cancellationToken);

                            return result{{entity}};
                        }
                    }       


                }
                """;
    }

    FormattableString GenerateModels(string projectName, string entity, string entityPlural, string entityLower)
    {

        return $$"""
                namespace {{projectName}}Application.Common.Models.{{entityPlural}}
                {
                    public class {{entity}}Model
                    {
                        public long Id { get; set; }
                        public string Name { get; set; }
                    }
                }
                
                """;
    }

    FormattableString GenerateGetQuery(string projectName, string entity, string entityPlural, string entityLower)
    {
        return $$"""
                using AutoMapper;
                using AutoMapper.QueryableExtensions;
                using MediatR;
                using {{projectName}}Application.Common.Models.{{entityPlural}};
                using {{projectName}}Application.Interfaces;
                using {{projectName}}Domain.Entities;
                using TaskManagementService.CommonLib.Extentions;
                using TaskManagementService.CommonLib.Requests;


                namespace {{projectName}}Application.UseCase.{{entityPlural}}.Queries.Get{{entityPlural}}
                {
                    public class Get{{entityPlural}}Query : PaginatedListRequest<{{entity}}Model>
                    {
                        public long[]? Ids { get; set; }
                        public string[]? Names { get; set; }
                    }

                    public class Get{{entityPlural}}QueryHandler : IRequestHandler<Get{{entityPlural}}Query, PaginatedList<{{entity}}Model>>
                    {
                        private readonly IApplicationDbContext context;
                        private readonly IMapper mapper;

                        public Get{{entityPlural}}QueryHandler(IApplicationDbContext context, IMapper mapper)
                        {
                            this.context = context;
                            this.mapper = mapper;
                        }

                        public async Task<PaginatedList<{{entity}}Model>> Handle(Get{{entityPlural}}Query query, CancellationToken cancellationToken)
                        {
                            IQueryable<{{entity}}> queue = context.{{entityPlural}};

                            if (query.Ids?.Length > 0)
                            {
                                queue = queue.Where(x => query.Ids.Any(b => b == x.Id));
                            }

                            if (query.Names?.Length > 0)
                            {
                                query.Names = Array.ConvertAll(query.Names, x => x.ToLower());
                                queue = queue.Where(x => query.Names.Contains(x.Name.ToLower()));
                            }

                            var {{entityLower}} = await queue
                                .OrderBy(x => x.Id)
                                .ProjectTo<{{entity}}Model>(mapper.ConfigurationProvider)
                                .PaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);

                            return {{entityLower}};
                        }
                    }

                }
                """;
    }

    FormattableString GenerateGetQueryValidator(string projectName, string entity, string entityPlural)
    {
        return $$"""
                using FluentValidation;
                using Shop24.{{projectName}}Application.Common.Models.{{entityPlural}};
                using TaskManagementService.CommonLib.Validation;

                namespace Shop24.{{projectName}}Application.UseCase.{{entityPlural}}.Queries.Get{{entityPlural}}
                {
                    public class Get{{entityPlural}}QueryValidator : AbstractValidator<Get{{entityPlural}}Query>
                    {
                        public Get{{entityPlural}}QueryValidator()
                        {
                            Include(new PaginatedListRequestValidator<{{entity}}Model>());
                        }
                    }
                }
                """;
    }

    FormattableString GenerateMapping(string projectName, string entity, string entityPlural, string entityLower)
    {
        return $$"""
                using AutoMapper;
                using {{projectName}}Application.Common.Models.{{entityPlural}};
                using {{projectName}}Application.UseCase.{{entityPlural}}.Commands.Create{{entity}};
                using {{projectName}}Domain.Entities;

                namespace {{projectName}}Application.Common.Mapping
                {
                    public class {{entity}}Profile : Profile
                    {
                        public {{entity}}Profile()
                        {
                            CreateMap<Create{{entity}}Command, {{entity}}>();
                            CreateMap<{{entity}}, {{entity}}Model>();
                        }
                    }
                }
                """;
    }


    FormattableString GenerateResultConsumerGetQuery(string projectName, string entity, string entityPlural, string entityLower)
    {
        return $$"""
            using {{projectName}}Application.Common.Models.{{entityPlural}};
            using TaskManagementService.CommonLib.Commands;
            using TaskManagementService.CommonLib.CustomAttributes;

            namespace {{projectName}}Application.ResultConsumer.{{entityPlural}}.Queries
            {
                public class Get{{entityPlural}}QueryResult : PaginatedListCommand<{{entity}}Model>
                { }
            }
            """;
    }

    FormattableString GenerateCreateValidator(string projectName, string entity, string entityPlural, string entityLower)
    {
        return $$"""
            using FluentValidation;
            using {{projectName}}Application.Extensions;

            namespace {{projectName}}Application.UseCase.{{entityPlural}}.Commands.Create{{entity}}
            {
                public class Create{{entity}}Validator : AbstractValidator<Create{{entity}}Command>
                {
                    public Create{{entity}}Validator()
                    {
                       RuleFor(c => c.Name).NotEmptyOrWhiteSpace().WithMessage("Укажите название");
                    }
                }
            }
            
            """;
    }


    FormattableString GenerateUpdateValidator(string projectName, string entity, string entityPlural, string entityLower)
    {
        return $$"""
            using FluentValidation;
            using {{projectName}}Application.Extensions;

            namespace {{projectName}}Application.UseCase.{{entityPlural}}.Commands.Update{{entity}}
            {
                public class Update{{entity}}Validator: AbstractValidator<Update{{entity}}Command>
                {
                    public Update{{entity}}Validator()
                    {
                        RuleFor(c => c.Id)
                            .NotEmpty().WithMessage("Укажите Id")
                            .GreaterThan(0).WithMessage("Id должен быть больше 0");

                        RuleFor(c => c.Name).NotEmptyOrWhiteSpace().WithMessage("Укажите название");

                    }
                }
            }
            """;
    }


    FormattableString GenerateDeleteValidator(string projectName, string entity, string entityPlural, string entityLower)
    {
        return $$"""
            using FluentValidation;

            namespace {{projectName}}Application.UseCase.{{entityPlural}}.Commands.Delete{{entity}}
            {
                public class Delete{{entity}}Validator : AbstractValidator<Delete{{entity}}Command>
                {
                    public Delete{{entity}}Validator() 
                    {
                        RuleFor(c => c.Id)
                        .NotEmpty().WithMessage("Укажите Id")
                        .GreaterThan(0).WithMessage("Id должен быть больше 0");
                    }
                }
            }
            """;
    }

    FormattableString GenerateConfigurationService(string projectName, string entity, string entityPlural, string entityLower)
    {
        return $$"""
            using MediatR;
            using Microsoft.Extensions.DependencyInjection;
            using TaskManagementService.CommonLib.CommandHandlers;
            using TaskManagementService.CommonLib.EventHandlers;
            using TaskManagementService.CommonLib.PaginatedList;
            using TaskManagementService.CommonLib.ResultHandlers;
            using {{projectName}}Application.Common.Models.{{entityPlural}};
            using {{projectName}}Application.UseCase.{{entityPlural}}.Commands.Create{{entity}};
            using {{projectName}}Application.UseCase.{{entityPlural}}.Commands.Delete{{entity}};
            using {{projectName}}Application.UseCase.{{entityPlural}}.Commands.Update{{entity}};
            using {{projectName}}Application.UseCase.{{entityPlural}}.Queries.Get{{entityPlural}};
            using {{projectName}}Application.ResultConsumer.{{entityPlural}}.Events.{{entity}}Created;
            using {{projectName}}Application.ResultConsumer.{{entityPlural}}.Events.{{entity}}Deleted;
            using {{projectName}}Application.ResultConsumer.{{entityPlural}}.Events.{{entity}}Updated;
            using {{projectName}}Application.ResultConsumer.{{entityPlural}}.Queries;

            namespace {{projectName}}Application
            {
                services.Add{{entity}}HandlerScope(); // перенести в ConfigureServices.cs

                public static class Configure{{entity}}Handler
                {
                    public static IServiceCollection Add{{entity}}HandlerScope(this IServiceCollection services)
                    {
                        // Commands
                        services.AddScoped(typeof(IRequestHandler<Create{{entity}}Command, {{entity}}Model>), typeof(BaseCommandHandler<Create{{entity}}Command, {{entity}}Model>));
                        services.AddScoped(typeof(IRequestHandler<Update{{entity}}Command, {{entity}}Model>), typeof(BaseCommandHandler<Update{{entity}}Command, {{entity}}Model>));
                        services.AddScoped(typeof(IRequestHandler<Delete{{entity}}Command, {{entity}}Model>), typeof(BaseCommandHandler<Delete{{entity}}Command, {{entity}}Model>));

                        // Queries
                        services.AddScoped(typeof(IRequestHandler<Get{{entityPlural}}Query, PaginatedList<{{entity}}Model>>), typeof(BaseGetManyCommandHandler<Get{{entityPlural}}Query, {{entity}}Model>));

                        // Events
                        services.AddScoped(typeof(IRequestHandler<{{entity}}CreatedEvent>), typeof(BaseEventHandler<{{entity}}CreatedEvent, {{entity}}Model>));
                        services.AddScoped(typeof(IRequestHandler<{{entity}}UpdatedEvent>), typeof(BaseEventHandler<{{entity}}UpdatedEvent, {{entity}}Model>));
                        services.AddScoped(typeof(IRequestHandler<{{entity}}DeletedEvent>), typeof(BaseEventHandler<{{entity}}DeletedEvent, {{entity}}Model>));

                        // Query result
                        services.AddScoped(typeof(IRequestHandler<Get{{entityPlural}}QueryResult>), typeof(QueryResultHandler<Get{{entityPlural}}QueryResult, {{entity}}Model>));

                        return services;
                    }        
                }
            }            
            """;
    }
}