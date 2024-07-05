using Intelligent_Document_Processing_and_Analysis_API.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Intelligent_Document_Processing_and_Analysis_API.DbContexts;

public class SQLiteDbContext(DbContextOptions<SQLiteDbContext> options) : DbContext(options)
{
    public DbSet<LlmInteraction> LlmInteractions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        try
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LlmInteraction>()
                .Property(e => e.Timestamp)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error at class: {class}, method: {method}", nameof(SQLiteDbContext), nameof(OnModelCreating));
        }
    }
}