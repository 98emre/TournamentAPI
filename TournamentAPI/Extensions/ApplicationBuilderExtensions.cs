using Microsoft.EntityFrameworkCore;
using TournamentAPI.Data.Data;

namespace TournamentAPI.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task SeedDataAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TournamentAPIContext>();

                await context.Database.EnsureDeletedAsync();
                await context.Database.MigrateAsync();

                try
                {
                    await SeedData.InitAsync(context);
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            }

        }
    }
}
