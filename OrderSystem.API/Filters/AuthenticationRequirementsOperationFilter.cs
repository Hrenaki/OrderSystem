using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OrderSystem.API.Filters
{
    public class AuthenticationRequirementsOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (!context.MethodInfo.CustomAttributes.Any(attr => attr.AttributeType.IsAssignableFrom(typeof(AuthorizeAttribute))))
                return;

            if (operation.Security is null)
                operation.Security = new List<OpenApiSecurityRequirement>();

            var scheme = new OpenApiSecurityScheme 
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            };

            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [scheme] = new List<string>()
            });
        }
    }
}