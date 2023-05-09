using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NextOptimization.Data
{
    public class NextOptimizationContext : IdentityDbContext/*<User>*/, IDataProtectionKeyContext
    {
        public NextOptimizationContext()
        {
        }

        public NextOptimizationContext(DbContextOptions<NextOptimizationContext> options)
        : base(options)
        {
        }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //

            base.OnModelCreating(modelBuilder);
        }
    }
}
