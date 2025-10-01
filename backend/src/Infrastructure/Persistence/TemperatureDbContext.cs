using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class TemperatureDbContext : DbContext
    {
        public TemperatureDbContext(DbContextOptions<TemperatureDbContext> options)
            : base(options) { }

        public DbSet<Formato> Formatos { get; set; }
        public DbSet<Registro> Registros { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Formato>()
                .HasMany(f => f.Registros)
                .WithOne(r => r.Formato!)
                .HasForeignKey(r => r.FormatoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Formato>().ToTable("Formatos");
            modelBuilder.Entity<Registro>().ToTable("Registros");
        }
    }
}
