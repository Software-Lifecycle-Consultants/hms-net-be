using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

/// <summary>
/// A custom operation filter to handle FromForm parameters. This will inform Swashbuckle to treat these parameters correctly.
/// When using FromForm to bind a model in an ASP.NET Core controller, you may encounter issues with Swagger not correctly displaying the model structure,
/// especially when dealing with nested objects. To address this, ensure that Swashbuckle is configured to understand FromForm parameters and properly generate the necessary schema.
/// </summary>


using System.Linq;

public class FromFormOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var parameterDescriptions = context.ApiDescription.ParameterDescriptions;
        var formParameters = parameterDescriptions
            .Where(p => p.Source.Id == "FormFile" || p.Source.Id == "Form")
            .ToList();

        foreach (var formParameter in formParameters)
        {
            var schema = context.SchemaGenerator.GenerateSchema(formParameter.Type, context.SchemaRepository);
            foreach (var property in schema.Properties)
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = property.Key,
                    In = ParameterLocation.Query,
                    Schema = property.Value,
                    Required = property.Value.Nullable == false
                });
            }
        }
    }
}

