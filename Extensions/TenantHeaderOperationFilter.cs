using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class TenantHeaderOperationFilter : IOperationFilter
{
  public void Apply(OpenApiOperation operation, OperationFilterContext context)
  {
    // Add the "Tenant" header to the request if the endpoint is not /api/Tenants
    if (!context.ApiDescription.RelativePath.Contains("api/Tenants"))
    {
      operation.Parameters.Add(new OpenApiParameter
      {
        Name = "Tenant",
        In = ParameterLocation.Header,
        Required = true,
        Description = "Tenant ID header. Enter your Tenant ID here.",
        Schema = new OpenApiSchema
        {
          Type = "string"
        }
      });
    }
  }
}
