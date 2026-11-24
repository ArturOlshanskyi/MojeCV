using System.ComponentModel.DataAnnotations;

namespace MojeCV.ViewModels;

public class ReservationCreateViewModel
{
    public int ServiceId { get; set; }
    public string ServiceName { get; set; } = "";

    [Required(ErrorMessage = "Wybierz termin")]
    [Display(Name = "Termin")]
    public int ReservationSlotId { get; set; }

    [Display(Name = "Notatka")]
    [StringLength(500)]
    public string? Notes { get; set; }
}
