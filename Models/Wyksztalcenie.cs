using System.ComponentModel.DataAnnotations;

namespace MojeCV.Models;

public class Wyksztalcenie
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Podaj nazwę uczelni")]
    [StringLength(150, ErrorMessage = "Nazwa uczelni nie może przekraczać 150 znaków")]
    public string Uczelnia { get; set; } = "";

    [Required(ErrorMessage = "Podaj kierunek studiów")]
    [StringLength(150, ErrorMessage = "Nazwa kierunku nie może przekraczać 150 znaków")]
    public string Kierunek { get; set; } = "";

    [StringLength(80, ErrorMessage = "Poziom nie może przekraczać 80 znaków")]
    public string? Poziom { get; set; }

    [Display(Name = "Od")]
    [DataType(DataType.Date)]
    public DateTime DataOd { get; set; }

    [Display(Name = "Do")]
    [DataType(DataType.Date)]
    public DateTime? DataDo { get; set; }
}
