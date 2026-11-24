using System.ComponentModel.DataAnnotations;

namespace MojeCV.Models;

public class Osoba
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Podaj imię")]
    [StringLength(100)]
    public string Imie { get; set; } = "";

    [Required(ErrorMessage = "Podaj nazwisko")]
    [StringLength(100)]
    public string Nazwisko { get; set; } = "";

    [StringLength(100)]
    public string Stanowisko { get; set; } = "";

    [Required(ErrorMessage = "Podaj adres e-mail")]
    [EmailAddress(ErrorMessage = "Wprowadź poprawny adres e-mail")]
    [StringLength(200)]
    public string Email { get; set; } = "";

    [Phone(ErrorMessage = "Wprowadź poprawny numer telefonu")]
    public string? Telefon { get; set; }

    [StringLength(100)]
    public string? Miasto { get; set; }

    [Display(Name = "O mnie")]
    [StringLength(2000)]
    public string? OMnie { get; set; }
}
