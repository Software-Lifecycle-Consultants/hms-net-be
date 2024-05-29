using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

/// <summary>
/// A custom operation filter to handle FromForm parameters. This will inform Swashbuckle to treat these parameters correctly.
/// When using FromForm to bind a model in an ASP.NET Core controller, you may encounter issues with Swagger not correctly displaying the model structure,
/// especially when dealing with nested objects. To address this, ensure that Swashbuckle is configured to understand FromForm parameters and properly generate the necessary schema.
/// </summary>

public class FromFormOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var formParameters = context.MethodInfo.GetParameters()
            .Where(p => p.GetCustomAttributes(typeof(FromFormAttribute), false).Any());

        foreach (var parameter in formParameters)
        {
            var schema = context.SchemaGenerator.GenerateSchema(parameter.ParameterType, context.SchemaRepository);
            AddPropertiesToOperation(operation, schema, string.Empty);
        }
    }

    private void AddPropertiesToOperation(OpenApiOperation operation, OpenApiSchema schema, string prefix)
    {
        foreach (var property in schema.Properties)
        {
            var propertyName = string.IsNullOrEmpty(prefix) ? property.Key : $"{prefix}.{property.Key}";
            var propertySchema = property.Value;

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = propertyName,
                In = ParameterLocation.Query,
                Schema = propertySchema,
                Required = schema.Required.Contains(property.Key)
            });

            if (propertySchema.Type == "object" && propertySchema.Properties.Count > 0)
            {
                AddPropertiesToOperation(operation, propertySchema, propertyName);
            }
        }
    }
}
