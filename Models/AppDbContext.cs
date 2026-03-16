using Microsoft.EntityFrameworkCore;

namespace asp_hub_kt6.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name).HasMaxLength(100).IsRequired();
            });
        }
    }
}