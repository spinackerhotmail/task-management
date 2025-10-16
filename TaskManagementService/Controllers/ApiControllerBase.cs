using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagementService.Controllers;

[ApiController]
[Route("tms/v{v:apiVersion}/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender mediator = null!;
    protected ISender Mediator => mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}