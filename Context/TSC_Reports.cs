using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UplLogEmail.Models.Tables;

namespace UplLogEmail.Context;

public class TSC_Reports(IConfiguration config) : DbContext
{
    public DbSet<StatesTable> StatesTable { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            $"{config.GetConnectionString("BaseConnectionString")}Initial Catalog=TSC_Reports;"
        );
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
