using Microsoft.EntityFrameworkCore;

namespace Intelligent_Document_Processing_and_Analysis_API.DbContexts;

public class SQLiteDbContext(DbContextOptions<SQLiteDbContext> options) : DbContext(options)
{
}