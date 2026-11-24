using System.ComponentModel.DataAnnotations;

namespace MojeCV.Models;

public class Reservation
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Termin")]
    public int ReservationSlotId { get; set; }
    public ReservationSlot? ReservationSlot { get; set; }

    [Required]
    [Display(Name = "Usługa")]
    public int ServiceId { get; set; }
    public Service? Service { get; set; }

    [Required]
    public string UserId { get; set; } = "";
    public AppUser? User { get; set; }

    [Display(Name = "Status")]
    [Required]
    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

    [Display(Name = "Notatka")]
    [StringLength(500)]
    public string? Notes { get; set; }

    [Display(Name = "Data utworzenia")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public enum ReservationStatus
{
    Pending,
    Approved,
    Rejected,
    Cancelled
}
