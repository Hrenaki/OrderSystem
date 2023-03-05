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
            var hasAuthorizeAttribute = context.MethodInfo.CustomAttributes.Any(attr => attr.AttributeType.IsAssignableFrom(typeof(AuthorizeAttribute)));
            var classType = context.MethodInfo.DeclaringType;
            hasAuthorizeAttribute = hasAuthorizeAttribute ||
                                    classType is not null && classType.CustomAttributes.Any(attr => attr.AttributeType.IsAssignableFrom(typeof(AuthorizeAttribute)));
            if (!hasAuthorizeAttribute)
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