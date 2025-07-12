using System.ComponentModel.DataAnnotations;

namespace CityVilleDotnet.Api.Pages.Account;

public class RegisterInputModel
{
    [Required(ErrorMessage = "Username is required")]
    [Display(Name = "Username")]
    public required string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public required string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "Passwords don't match")]
    public required string ConfirmPassword { get; set; }
}