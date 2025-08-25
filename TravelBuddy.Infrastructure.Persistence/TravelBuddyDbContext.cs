using Microsoft.EntityFrameworkCore;
using TravelBuddy.Domain.Entities;

namespace TravelBuddy.Infrastructure.Persistence
{
    public sealed class TravelBuddyDbContext(DbContextOptions<TravelBuddyDbContext> options) : DbContext(options)
    {
        public DbSet<Destination> Destinations => Set<Destination>();
        public DbSet<Reservation> Reservations => Set<Reservation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Destination>(b =>
            {
                b.ToTable("Destinations");
                b.HasKey(d => d.Id);
                b.Property(d => d.Name).IsRequired().HasMaxLength(200);
                b.Property(d => d.Country).IsRequired().HasMaxLength(100);
                b.Property(d => d.Description).HasMaxLength(1000);
                b.Property(d => d.AveragePrice).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<Reservation>(b =>
            {
                b.ToTable("Reservations");
                b.HasKey(r => r.Id);
                b.Property(r => r.CustomerName).IsRequired().HasMaxLength(200);
                b.Property(r => r.TravelDate).IsRequired();
                b.Property(r => r.NumberOfPeople).IsRequired();
                b.Property(r => r.TotalPrice).HasColumnType("decimal(18,2)");

                b.HasOne<Destination>()
                 .WithMany()
                 .HasForeignKey(r => r.DestinationId)
                 .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
