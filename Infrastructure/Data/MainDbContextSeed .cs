using Microsoft.AspNetCore.Builder;

namespace Infrastructure.Data
{
    public static class MainDbContextSeed
    {
        public static async Task InitializeDatabaseAsync(this WebApplication webApp)
        {
            using (var scope = webApp.Services.CreateScope())
            {
                using (var _context = scope.ServiceProvider.GetRequiredService<MainDbContext>())
                {
                    try
                    {
                        if (_context.Database.IsSqlServer())
                        {
                            await _context.Database.MigrateAsync();
                        }
                    }
                    catch (Exception)
                    {
                        //Log errors or do anything you think it's needed
                        throw;
                    }
                }
            }
        }
    }
}
