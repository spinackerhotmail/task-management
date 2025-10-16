using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TaskManagementService.CommonLib.Extentions;

namespace TaskManagementService.CommonLib.Swagger.Filters
{
    public class CamelCaseParameOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = [];
            }
            else
            {
                foreach (var item in operation.Parameters)
                {
                    item.Name = item.Name.ToCamelCase();
                }
            }
        }
    }
}
