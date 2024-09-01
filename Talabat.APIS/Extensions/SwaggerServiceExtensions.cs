using Microsoft.OpenApi.Models;

namespace Talabat.APIS.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api Demo", Version = "v1" });
                var securityscheme = new OpenApiSecurityScheme
                {
                    Description = "Jwt Authorization header using the bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "bearer"
                    }
                };

                c.AddSecurityDefinition("bearer", securityscheme);

                var securityRequirement = new OpenApiSecurityRequirement
                {
                    { securityscheme, new [] {"bearer"} }
                };

                c.AddSecurityRequirement(securityRequirement);

            });
            return services;
        }
    }
}
