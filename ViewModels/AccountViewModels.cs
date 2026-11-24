using System.ComponentModel.DataAnnotations;

namespace MojeCV.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Podaj imię")]
    [Display(Name = "Imię")]
    public string FirstName { get; set; } = "";

    [Required(ErrorMessage = "Podaj nazwisko")]
    [Display(Name = "Nazwisko")]
    public string LastName { get; set; } = "";

    [Required(ErrorMessage = "Podaj adres e-mail")]
    [EmailAddress(ErrorMessage = "Niepoprawny adres e-mail")]
    [Display(Name = "E-mail")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Podaj hasło")]
    [DataType(DataType.Password)]
    [Display(Name = "Hasło")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Hasło musi mieć co najmniej 6 znaków")]
    public string Password { get; set; } = "";

    [DataType(DataType.Password)]
    [Display(Name = "Powtórz hasło")]
    [Compare(nameof(Password), ErrorMessage = "Hasła muszą być takie same")]
    public string ConfirmPassword { get; set; } = "";
}

public class LoginViewModel
{
    [Required(ErrorMessage = "Podaj adres e-mail")]
    [EmailAddress]
    [Display(Name = "E-mail")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Podaj hasło")]
    [DataType(DataType.Password)]
    [Display(Name = "Hasło")]
    public string Password { get; set; } = "";

    [Display(Name = "Zapamiętaj mnie")]
    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
}
