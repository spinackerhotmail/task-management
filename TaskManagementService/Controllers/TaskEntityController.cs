using MediatR;
using TaskManagementService.Controllers;
using TaskManagementService.CommonLib.Exceptions;
using TaskManagementService.Application.UseCase.TaskEntities.Commands.CreateTaskEntity;
using TaskManagementService.Application.UseCase.TaskEntities.Commands.DeleteTaskEntity;
using TaskManagementService.Application.UseCase.TaskEntities.Commands.UpdateTaskEntity;
using TaskManagementService.Application.UseCase.TaskEntities.Queries.GetTaskEntities;
using Microsoft.AspNetCore.Mvc;
using TaskManagementService.CommonLib.Requests;
using TaskManagementService.Application.UseCase.TaskEntities.Models;

namespace Controllers
{
    public class TaskEntityController : ApiControllerBase
    {
        /// <summary>
        /// Получить данные
        /// </summary>
        /// <param name="query">Фильтр</param>
        /// <returns><see cref="PaginatedList{T}"/> - Список для пагинации.
        /// <para><see cref="TaskEntityModel"/></para></returns>
        [HttpGet]
        public async Task<ActionResult<PaginatedList<TaskEntityModel>>> Get([FromQuery] GetTaskEntitiesQuery query)
        {
            return await Mediator.Send(query);
        }

        /// <summary>
        /// Получить данные по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="TaskEntityModel"/></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskEntityModel>> GetOne(Guid id)
        {
            var result = await Mediator.Send(new GetTaskEntitiesQuery { Ids = [id] });

            if (result?.Items.Count <= 0)
                throw new NotFoundException("Не найден");
            else
                return result?.Items.FirstOrDefault();

        }

        /// <summary>
        /// Создать
        /// </summary>
        /// <param name="command"><see cref="CreateTaskEntityCommand"/></param>
        /// <returns><see cref="TaskEntityModel"/></returns>
        [HttpPost]
        public async Task<ActionResult<TaskEntityModel>> Create(CreateTaskEntityCommand command)
        {
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Обновить данные
        /// </summary>
        /// <returns><see cref="TaskEntityModel"/> </returns>
        [HttpPut]
        public async Task<ActionResult<TaskEntityModel>> Update(UpdateTaskEntityCommand command)
        {
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Удалить
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns><see cref="TaskEntityModel"/></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<TaskEntityModel>> Delete(Guid id)
        {
            return await Mediator.Send(new DeleteTaskEntityCommand() { Id = id });
        }
    }
}

