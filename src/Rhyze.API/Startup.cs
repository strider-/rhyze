using AspNetCore.Firebase.Authentication.Extensions;
using FluentMigrator.Runner;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rhyze.API.Extensions;
using Rhyze.Data.Migrations;
using System.Reflection;

namespace Rhyze.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connStr = Configuration.GetSection("Database")["ConnectionString"];
            services.AddFluentMigratorCore()
                    .ConfigureRunner(rb =>
                        rb.AddSqlServer()
                          .WithGlobalConnectionString(connStr)
                          .ScanIn(typeof(CreateDatabase).Assembly).For.Migrations())
                    .AddLogging(lb => lb.AddFluentMigratorConsole());

            services.AddControllers(options => options.Filters.Add(new AuthorizeFilter()));

            var jwtConfig = Configuration.GetSection("Authentication:Jwt");
            services.AddFirebaseAuthentication(jwtConfig["Issuer"], jwtConfig["Audience"]);

            services.AddMediatR(Assembly.GetExecutingAssembly());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );

            app.UseAuthentication()
               .UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseMigrations();
        }
    }
}