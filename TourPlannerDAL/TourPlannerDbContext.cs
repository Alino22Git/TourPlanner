using Microsoft.EntityFrameworkCore;
using Models;

namespace TourPlannerDAL
{
    public class TourPlannerDbContext : DbContext
    {
        public TourPlannerDbContext(DbContextOptions<TourPlannerDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tour> Tours { get; set; }
        public DbSet<TourLog> TourLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tour>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.From).HasMaxLength(100);
                entity.Property(e => e.To).HasMaxLength(100);
                entity.Property(e => e.Distance).HasMaxLength(50);
                entity.Property(e => e.Time).HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.TransportType).HasMaxLength(500);
                entity.Property(e => e.Popularity).HasMaxLength(500);
                entity.Property(e => e.ChildFriendliness).HasMaxLength(500);
            });

            modelBuilder.Entity<TourLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.Comment).HasMaxLength(500);
                entity.Property(e => e.Difficulty).HasMaxLength(50);
                entity.Property(e => e.TotalDistance).IsRequired().HasColumnType("double precision");
                entity.Property(e => e.TotalTime).IsRequired().HasColumnType("double precision");
                entity.Property(e => e.Rating).HasMaxLength(50);
                entity.Property(e => e.Weather).HasMaxLength(50);
                entity.HasOne(t => t.Tour)
                    .WithMany(t => t.TourLogs)
                    .HasForeignKey(t => t.TourId);
            });
        }
    }
}