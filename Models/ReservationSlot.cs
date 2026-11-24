using System.ComponentModel.DataAnnotations;

namespace MojeCV.Models;

public class ReservationSlot
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Usługa")]
    public int ServiceId { get; set; }
    public Service? Service { get; set; }

    [Required]
    [Display(Name = "Data i godzina")]
    public DateTime StartTime { get; set; }

    [Display(Name = "Maksymalna liczba miejsc")]
    [Range(1, 100)]
    public int Capacity { get; set; } = 1;

    [Display(Name = "Dostępny")]
    public bool IsAvailable { get; set; } = true;

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
