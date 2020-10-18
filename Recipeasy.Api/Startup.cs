using System;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Recipeasy.Api.Contexts;
using Recipeasy.Api.Interfaces.Services;
using Recipeasy.Api.Services;

namespace Recipeasy.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = Configuration["Auth0Authority"];
                options.Audience = Configuration["Auth0Audience"];
            });
            
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder
                            .WithOrigins(
                                "http://localhost:4200",
                                "https://silly-mahavira-4f9bf8.netlify.app")
                            .WithMethods("PUT", "DELETE", "GET", "OPTIONS", "POST")
                            .AllowAnyHeader();
                    });
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                {
                    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                    string connStr;

                    // Depending on if in development or production, use either Heroku-provided
                    // connection string, or development connection string from env var.
                    if (env == "Development")
                    {
                        // Use connection string from file.
                        connStr = Configuration.GetConnectionString("ApplicationDbContext");
                    }
                    else
                    {
                        // Use connection string provided at runtime by Heroku.
                        var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

                        // Parse connection URL to connection string for Npgsql
                        connUrl = connUrl.Replace("postgres://", string.Empty);
                        var pgUserPass = connUrl.Split("@")[0];
                        var pgHostPortDb = connUrl.Split("@")[1];
                        var pgHostPort = pgHostPortDb.Split("/")[0];
                        var pgDb = pgHostPortDb.Split("/")[1];
                        var pgUser = pgUserPass.Split(":")[0];
                        var pgPass = pgUserPass.Split(":")[1];
                        var pgHost = pgHostPort.Split(":")[0];
                        var pgPort = pgHostPort.Split(":")[1];

                        connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb}";
                    }

                    // Whether the connection string came from the local development configuration file
                    // or from the environment variable from Heroku, use it to set up your DbContext.
                    options.UseNpgsql(connStr);
                });

            AddRecipeasyDependencies(services);

            services.AddAutoMapper(typeof(Startup));

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "APIs for Recipeasy SPA",
                    Version = "v1"
                });
            });
        }

        private void AddRecipeasyDependencies(IServiceCollection services)
        {
            services.AddTransient<IRecipesService, RecipesService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseRouting();
            app.UseCors();
            
            app.UseAuthentication();
            app.UseAuthorization();
                
            app.UseEndpoints(endpoints => endpoints.MapControllers());
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseHttpsRedirection();
        }
    }
}
