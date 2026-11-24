using System.ComponentModel.DataAnnotations;

namespace MojeCV.Models;

public class Film
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Podaj tytuł filmu")]
    [StringLength(200, ErrorMessage = "Tytuł może mieć maksymalnie 200 znaków")]
    [Display(Name = "Tytuł")]
    public string Tytul { get; set; } = "";

    [Required(ErrorMessage = "Podaj długość filmu")]
    [Range(1, 1440, ErrorMessage = "Długość musi być liczbą od 1 do 1440 minut")]
    [Display(Name = "Długość (minuty)")]
    public int DlugoscMinuty { get; set; }

    [Required(ErrorMessage = "Podaj reżysera")]
    [StringLength(150, ErrorMessage = "Nazwisko reżysera może mieć maksymalnie 150 znaków")]
    [Display(Name = "Reżyser")]
    public string Rezyser { get; set; } = "";
}
