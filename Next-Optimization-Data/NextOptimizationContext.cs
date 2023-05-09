using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NextOptimization.Data.Models;
using NextOptimization.Data.Models.SoftDelete;

namespace NextOptimization.Data
{
    public class NextOptimizationContext : IdentityDbContext<User>, IDataProtectionKeyContext
    {
        public NextOptimizationContext()
        {
        }

        public NextOptimizationContext(DbContextOptions<NextOptimizationContext> options)
        : base(options)
        {
        }

        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Package> Packages { get; set; }
        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd()
                      .HasDefaultValueSql("NEWID()");

                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd()
                      .HasDefaultValueSql("NEWID()");

                entity.HasOne(e => e.Buyer)
                      .WithMany(e => e.AppointmentHistory)
                      .HasForeignKey(e => e.BuyerId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Package)
                      .WithMany()
                      .HasForeignKey(e => e.PackageId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd()
                      .HasDefaultValueSql("NEWID()");

                entity.HasOne(e => e.Reviewer)
                      .WithMany(e => e.Reviews)
                      .HasForeignKey(e => e.ReviewerId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Package>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("Id")
                      .HasColumnType("int")
                      .ValueGeneratedOnAdd()
                      .UseIdentityColumn();
            });

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            SoftDelete();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SoftDelete();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SoftDelete()
        {
            ChangeTracker.DetectChanges();

            foreach (var item in ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted && (e.Entity is ISoftDelete)))
            {
                item.State = EntityState.Modified;
                item.CurrentValues["IsDeleted"] = true;
            }
        }
    }
}
