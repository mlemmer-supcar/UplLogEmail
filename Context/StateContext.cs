using Microsoft.EntityFrameworkCore;
using UplLogEmail.Models.Tables;

namespace UplLogEmail.Context
{
    public class StateContext(DbContextOptions<StateContext> options) : DbContext(options)
    {
        public DbSet<FacilityTable> FacilityTable { get; set; } = null!;
        public DbSet<PccFacilityTable> PccFacilityTable { get; set; } = null!;
        public DbSet<FacContactTable> FacContactTable { get; set; } = null!;
        public DbSet<ClientInfoTable> ClientInfoTable { get; set; } = null!;
        public DbSet<PccUploadLogTable> PccUploadLogTable { get; set; } = null!;
        public DbSet<ProviderTable> ProviderTable { get; set; } = null!;
        public DbSet<PhqTable> PhqTable { get; set; } = null!;
        public DbSet<BimsTable> BimsTable { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
