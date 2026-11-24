using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MojeCV.Models;

namespace MojeCV.Data;

public class CvDbContext : IdentityDbContext<AppUser>
{
    public CvDbContext(DbContextOptions<CvDbContext> options) : base(options)
    {
    }

    public DbSet<Osoba> Osoby => Set<Osoba>();
    public DbSet<Doswiadczenie> Doswiadczenia => Set<Doswiadczenie>();
    public DbSet<Umiejetnosc> Umiejetnosci => Set<Umiejetnosc>();
    public DbSet<Wyksztalcenie> Wyksztalcenia => Set<Wyksztalcenie>();
    public DbSet<Film> Filmy => Set<Film>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<ReservationSlot> ReservationSlots => Set<ReservationSlot>();
    public DbSet<Reservation> Reservations => Set<Reservation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Service>(entity =>
        {
            entity.Property(s => s.Name).HasMaxLength(100).IsRequired();
            entity.Property(s => s.Description).HasMaxLength(1000).IsRequired();
            entity.Property(s => s.Price).HasPrecision(10, 2);
        });

        modelBuilder.Entity<ReservationSlot>(entity =>
        {
            entity.HasOne(s => s.Service)
                .WithMany(s => s.Slots)
                .HasForeignKey(s => s.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(r => r.Service)
                .WithMany(s => s.Reservations)
                .HasForeignKey(r => r.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.ReservationSlot)
                .WithMany(s => s.Reservations)
                .HasForeignKey(r => r.ReservationSlotId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(r => new { r.ReservationSlotId, r.UserId });
        });

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.Property(u => u.FirstName).HasMaxLength(100);
            entity.Property(u => u.LastName).HasMaxLength(100);
        });

        modelBuilder.Entity<Osoba>(entity =>
        {
            entity.Property(o => o.Imie).HasMaxLength(100);
            entity.Property(o => o.Nazwisko).HasMaxLength(100);
            entity.Property(o => o.Stanowisko).HasMaxLength(150);
            entity.Property(o => o.Email).HasMaxLength(150);
            entity.Property(o => o.Miasto).HasMaxLength(100);
        });

        modelBuilder.Entity<Doswiadczenie>(entity =>
        {
            entity.Property(d => d.Stanowisko).HasMaxLength(150);
            entity.Property(d => d.Firma).HasMaxLength(150);
        });

        modelBuilder.Entity<Umiejetnosc>(entity =>
        {
            entity.Property(u => u.Nazwa).HasMaxLength(100);
            entity.Property(u => u.Kategoria).HasMaxLength(80);
        });

        modelBuilder.Entity<Wyksztalcenie>(entity =>
        {
            entity.Property(w => w.Uczelnia).HasMaxLength(150);
            entity.Property(w => w.Kierunek).HasMaxLength(150);
            entity.Property(w => w.Poziom).HasMaxLength(80);
        });

        modelBuilder.Entity<Film>(entity =>
        {
            entity.Property(f => f.Tytul).HasMaxLength(200);
            entity.Property(f => f.Rezyser).HasMaxLength(150);
        });
    }
}
