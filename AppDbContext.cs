using Healthcare.Models;
using Microsoft.EntityFrameworkCore;

namespace Healthcare;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Schedule> Schedules => Set<Schedule>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn();
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");
            }
        );
        
        modelBuilder.Entity<Schedule>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn();
                entity.Property(e => e.DoctorId)
                    .IsRequired();
                entity.Property(e => e.DayId)
                    .IsRequired();
                entity.Property(e => e.From)
                    .IsRequired();
                entity.Property(e => e.To)
                    .IsRequired();
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");
            }
        );
    }
}