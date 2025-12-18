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
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Appointment> Appointments => Set<Appointment>();


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

        modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn();
                entity.Property(e => e.Name)
                    .IsRequired();
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");
            }
        );

        modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn();
                entity.Property(e => e.PatientId)
                    .IsRequired();
                entity.Property(e => e.Day)
                    .IsRequired();
                entity.Property(e => e.ScheduleId)
                    .IsRequired();
                entity.Property(e => e.Start)
                    .IsRequired();
                entity.Property(e => e.Duration)
                    .IsRequired();
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");
            }
        );
    }
}