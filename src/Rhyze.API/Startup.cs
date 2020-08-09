using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rhyze.API.Extensions;
using Rhyze.API.Filters;
using Rhyze.Core.Interfaces;
using Rhyze.Services;
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
            services.AddDataAccessLayer(Configuration.GetSection("Database"));

            services.AddControllers(options =>
            {
                options.Filters.Add(new AuthorizeFilter());

                // when implmenting the IRequireAnOwner interface on request models,
                // this attribute sets the OwnerId property to the authenticated user id.
                options.Filters.Add(new BindRequestModelsToUserAttribute());
            });

            services.AddJwtAuthentication(Configuration.GetSection("Authentication:Jwt"));

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddScoped<IUploadService, UploadService>()
                    .AddScoped<IBlobStore>(p => new AzureBlobStore(Configuration.GetValue<string>("Storage:ConnectionString")))
                    .AddScoped<IQueueService>(p => new AzureQueueService(Configuration.GetValue<string>("Queue:ConnectionString")));
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