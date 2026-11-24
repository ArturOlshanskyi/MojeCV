using System.ComponentModel.DataAnnotations;

namespace MojeCV.Models;

public class Service
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Podaj nazwę usługi")]
    [StringLength(100)]
    [Display(Name = "Nazwa usługi")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Podaj opis")]
    [StringLength(1000)]
    [Display(Name = "Opis")]
    public string Description { get; set; } = "";

    [Range(1, 10000, ErrorMessage = "Cena musi mieścić się w zakresie 1–10 000 zł")]
    [Display(Name = "Cena (zł)")]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }

    [Range(15, 480, ErrorMessage = "Czas trwania musi wynosić 15–480 minut")]
    [Display(Name = "Czas trwania (min)")]
    public int DurationMinutes { get; set; } = 60;

    [Display(Name = "Aktywna")]
    public bool IsActive { get; set; } = true;

    public ICollection<ReservationSlot> Slots { get; set; } = new List<ReservationSlot>();
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
