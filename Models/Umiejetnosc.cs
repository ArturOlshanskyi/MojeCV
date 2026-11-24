using System.ComponentModel.DataAnnotations;

namespace MojeCV.Models;

public class Umiejetnosc
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nazwa { get; set; } = "";

    [StringLength(100)]
    public string Kategoria { get; set; } = "";

    [Range(1, 5, ErrorMessage = "Poziom musi być w zakresie od 1 do 5")]
    public int Poziom { get; set; }   // 1–5
}
