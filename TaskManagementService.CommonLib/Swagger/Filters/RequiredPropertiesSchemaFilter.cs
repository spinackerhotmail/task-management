using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace TaskManagementService.CommonLib.Swagger.Filters
{
    public class RequiredPropertiesSchemaFilter : ISchemaFilter
    {
        private readonly CamelCasePropertyNamesContractResolver? camelCaseContractResolver;

        private readonly NullabilityInfoContext nullabilityInfoContext = new();

        public RequiredPropertiesSchemaFilter(bool camelCasePropertyNames)
        {
            camelCaseContractResolver = camelCasePropertyNames ? new CamelCasePropertyNamesContractResolver() : null;
        }

        /// <summary>
        /// Add to model.Required all properties where Nullable is false.
        /// </summary>
        public void Apply(OpenApiSchema model, SchemaFilterContext context)
        {
            var type = context.Type;

            foreach (var property in type.GetProperties())
            {
                // check if required attr or type is not nullable<>
                var isRequired = !IsNullable(property); // ||  property.GetCustomAttribute<RequiredAttribute>() != null;
                if (isRequired)
                {
                    var propertyName = PropertyName(property);
                    if (model.Properties.ContainsKey(propertyName))
                    {
                        model.Required.Add(propertyName);
                    }
                }
            }
        }

        private string PropertyName(PropertyInfo property)
        {
            return camelCaseContractResolver?.GetResolvedPropertyName(property.Name) ?? property.Name;
        }

        private bool IsNullable(PropertyInfo propertyInfo)
        {
            var nullabilityInfo = nullabilityInfoContext.Create(propertyInfo);

            return nullabilityInfo.WriteState == NullabilityState.Nullable;
        }
    }
}
