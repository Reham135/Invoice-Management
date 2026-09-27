using Microsoft.EntityFrameworkCore;

namespace InvoiceManagement.Infrastructure.Persistence;

public static class DbInitializer
{
    public static async Task InitializeAsync(AppDbContext context)
    {
        await context.Database.MigrateAsync();
    }
}
