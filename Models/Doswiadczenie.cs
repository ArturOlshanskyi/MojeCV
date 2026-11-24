using System.ComponentModel.DataAnnotations;

namespace MojeCV.Models;

public class Doswiadczenie
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Podaj stanowisko")]
    [StringLength(100)]
    public string Stanowisko { get; set; } = "";

    [Required]
    [StringLength(100)]
    public string Firma { get; set; } = "";

    [Display(Name = "Od")]
    [DataType(DataType.Date)]
    public DateTime DataOd { get; set; }

    [Display(Name = "Do")]
    [DataType(DataType.Date)]
    public DateTime? DataDo { get; set; }

    [StringLength(1000)]
    public string? Opis { get; set; }

    public string CzasTrwania
    {
        get
        {
            var koniec = DataDo ?? DateTime.Today;
            var miesiace = (koniec.Year - DataOd.Year) * 12 + koniec.Month - DataOd.Month;

            if (koniec.Day < DataOd.Day)
                miesiace--;

            if (miesiace <= 0)
                return "mniej niż 1 mies.";

            if (miesiace < 12)
                return $"{miesiace} mies.";

            var lata = miesiace / 12;
            var reszta = miesiace % 12;

            if (reszta == 0)
                return lata == 1 ? "1 rok" : $"{lata} lat";

            return lata == 1 ? $"1 rok {reszta} mies." : $"{lata} lat {reszta} mies.";
        }
    }
}
