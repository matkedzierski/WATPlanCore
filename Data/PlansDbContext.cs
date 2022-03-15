using Microsoft.EntityFrameworkCore;
using WATPlanCore.Models;

namespace WATPlanCore.Data;

public class PlansDbContext : DbContext
{
    public DbSet<HistoryEntry>? History { get; set; }

    public PlansDbContext(DbContextOptions<PlansDbContext> options) : base(options)
    {
        
    }
}