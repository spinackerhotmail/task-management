using System.IO;
using System.Text;
using System.Reflection;

//TODO: сворован с DAS. нужна правка юзингов, названий сущностей и некоторой логики.
//Тем не менее, можно использовать для создания базовых каталогов
public class AddEntity
{
    void Main(ICodegenContext context)
    {
        // TODO: заменить название на нужную сущность
        // ВАЖНО: после генерации вернуть на Test - если не вернуть, может включиться повторно генерация и создаст по новой файлы (затрет изменения)

        var entity = "TaskEntity";
        var entityPlural = "TaskEntities";

        var projectName = "TaskManagementService.";

        #region Generate service
        var servicePath = $"../Controllers";
        var serviceFile = $"{entity}Controller.cs";
        var serviceFilePath = $"{servicePath}/{serviceFile}";

        if (!File.Exists(serviceFilePath))
        {
            var contextCreate = new CodegenContext();
            contextCreate[serviceFile]
                .WriteLine(GenerateController(entity, entityPlural, projectName));

            contextCreate.SaveFiles(outputFolder: $"{servicePath}");

        }

        #endregion
    }

    FormattableString GenerateController(string entity, string entityPlural, string projectName)
    {
        return $$"""
            using MediatR;            
            using {{projectName}}Controllers;
            using TaskManagementService.CommonLib.Exceptions;
            using {{projectName}}Application.Common.Models.{{entityPlural}};
            using {{projectName}}Application.UseCase.{{entityPlural}}.Commands.Create{{entity}};
            using {{projectName}}Application.UseCase.{{entityPlural}}.Commands.Delete{{entity}};
            using {{projectName}}Application.UseCase.{{entityPlural}}.Commands.Update{{entity}};
            using {{projectName}}Application.UseCase.{{entityPlural}}.Queries.Get{{entityPlural}};
            using Microsoft.AspNetCore.Mvc;
            using TaskManagementService.CommonLib.Requests;

            namespace Controllers
            {
                public class {{entity}}Controller : ApiControllerBase
                {
                    /// <summary>
                    /// Получить данные 
                    /// </summary>
                    /// <param name="query">Фильтр</param>
                    /// <returns><see cref="PaginatedList{T}"/> - Список для пагинации.
                    /// <para><see cref="{{entity}}Model"/></para></returns>
                    [HttpGet]
                    public async Task<ActionResult<PaginatedList<{{entity}}Model>>> Get([FromQuery] Get{{entityPlural}}Query query)
                    {
                        return await Mediator.Send(query);
                    }

                    /// <summary>
                    /// Получить данные по Id
                    /// </summary>
                    /// <param name="id"></param>
                    /// <returns><see cref="{{entity}}Model"/></returns>
                    [HttpGet("{id}")]
                    public async Task<ActionResult<{{entity}}Model>> GetOne(long id)
                    {
                        var result = await Mediator.Send(new Get{{entityPlural}}Query { Ids = [id] });

                        if (result?.Items.Count <= 0)
                            throw new NotFoundException("Не найден");
                        else
                            return result?.Items.FirstOrDefault();

                    }

                    /// <summary>
                    /// Создать
                    /// </summary>
                    /// <param name="command"><see cref="Create{{entity}}Command"/></param>
                    /// <returns><see cref="{{entity}}Model"/></returns>
                    [HttpPost]
                    public async Task<ActionResult<{{entity}}Model>> Create(Create{{entity}}Command command)
                    {
                        return await Mediator.Send(command);
                    }

                    /// <summary>
                    /// Обновить данные 
                    /// </summary>
                    /// <returns><see cref="{{entity}}Model"/> </returns>
                    [HttpPut]
                    public async Task<ActionResult<{{entity}}Model>> Update(Update{{entity}}Command command)
                    {
                        return await Mediator.Send(command);
                    }

                    /// <summary>
                    /// Удалить
                    /// </summary>
                    /// <param name="id">Идентификатор</param>
                    /// <returns><see cref="{{entity}}Model"/></returns>
                    [HttpDelete("{id}")]
                    public async Task<ActionResult<{{entity}}Model>> Delete(long id)
                    {
                        return await Mediator.Send(new Delete{{entity}}Command() { Id = id });
                    }
                }
            }
            
            """;

    }
}