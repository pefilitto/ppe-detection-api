using System.Reflection;
using Microsoft.OpenApi.Models;

namespace ppe_detection_api.swagger;

public static class SwaggerConfig
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "PPE Detection API",
                Version = "v1",
                Description = "API para detecção de Equipamentos de Proteção Individual",
                Contact = new OpenApiContact
                {
                    Name = "Pedro Correia Felitto",
                    Email = "pedrofilitto111@gmail.com"
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }
        });

        return services;
    }

    public static WebApplication UseSwaggerConfiguration(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PPE Detection API v1");
            });
        }

        return app;
    }
}