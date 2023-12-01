using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace MoverCandidateTest.Controllers.Config;

public static class SwaggerConfig
{
    public static void SwaggerServicesConfig(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        // services.AddSwaggerGen();
        services.AddSwaggerGen(c =>
            {
                // Set the API version and title
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MoverCandidateTestSwaggerPage",
                    Version = "v1",
                    Description = "A description of your API"
                });

                // Add security definitions for API authentication
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    Description = "Enter your Bearer token in the format 'Bearer {token}'"
                });

                // Add security requirements for specific endpoints
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                });
            }
        );
    }

        public static void SwaggerAppConfig(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable Swagger middleware and UI
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API");
            });
        }
}