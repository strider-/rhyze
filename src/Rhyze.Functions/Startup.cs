using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Rhyze.Core.Interfaces;
using Rhyze.Data;
using Rhyze.Functions;
using Rhyze.Services;
using System;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Rhyze.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var connStr = Environment.GetEnvironmentVariable("Database:ConnectionString");

            builder.Services.AddScoped<ITagReader, TagLibReader>()
                            .AddScoped<IConnectionContext>(p => new SqlConnectionContext(connStr))
                            .AddScoped<IDatabase, Database>();
        }
    }
}
