using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Rhyze.API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Runs any pending database migrations, or rolls them back to a given version.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="rollDownTo">When set, rolls back migrations to the given version.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseMigrations(this IApplicationBuilder app, long? rollDownTo = null)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

                if (rollDownTo.HasValue)
                {
                    runner.MigrateDown(rollDownTo.Value);
                }
                else
                {
                    runner.MigrateUp();
                }
            }

            return app;
        }
    }
}