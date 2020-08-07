using FluentMigrator.Runner;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Rhyze.API.Security;
using Rhyze.Data;
using Rhyze.Data.Migrations;

namespace Rhyze.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds authentication with JWT bearer support.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configSection">The configuration section containing the issuer and audience</param>
        /// <returns></returns>
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfigurationSection configSection)
        {
            var issuer = configSection["Issuer"];
            var audience = configSection["Audience"];
            var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>($"{issuer}/.well-known/openid-configuration", new OpenIdConnectConfigurationRetriever());

            services.AddScoped<RhyzeBearerEvents>();

            services.AddAuthentication(o =>
                    {
                        o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer(o =>
                    {
                        o.IncludeErrorDetails = true;
                        o.RefreshOnIssuerKeyNotFound = true;
                        o.MetadataAddress = $"{issuer}/.well-known/openid-configuration";
                        o.ConfigurationManager = configurationManager;
                        o.Audience = audience;
                        o.EventsType = typeof(RhyzeBearerEvents);
                    });

            return services;
        }

        /// <summary>
        /// Adds and configures support for the data access layer, including database migrations.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configSection">The configuration section containing the database connection string.</param>
        /// <returns></returns>
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfigurationSection configSection)
        {
            var connStr = configSection["ConnectionString"];

            services.AddFluentMigratorCore()
                    .ConfigureRunner(rb =>
                        rb.AddSqlServer()
                          .WithGlobalConnectionString(connStr)
                          .ScanIn(typeof(CreateUsersTable).Assembly).For.Migrations())
                    .AddLogging(lb => lb.AddFluentMigratorConsole());


            services.AddScoped<IDatabase>(provider => new Database(connStr));

            return services;
        }
    }
}