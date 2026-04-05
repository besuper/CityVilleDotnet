using System.ComponentModel.DataAnnotations;

namespace CityVilleDotnet.Api.Pages.Account;

public class RegisterInputModel
{
    [Required(ErrorMessageResourceType = typeof(Resources.SharedResource), ErrorMessageResourceName = "UsernameRequired")]
    [MinLength(4, ErrorMessageResourceType = typeof(Resources.SharedResource), ErrorMessageResourceName = "UsernameMinLength")]
    [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessageResourceType = typeof(Resources.SharedResource), ErrorMessageResourceName = "UsernameInvalidChars")]
    [Display(Name = "Username", ResourceType = typeof(Resources.SharedResource))]
    public required string Username { get; set; }

    [Required(ErrorMessageResourceType = typeof(Resources.SharedResource), ErrorMessageResourceName = "PasswordRequired")]
    [DataType(DataType.Password)]
    [Display(Name = "Password", ResourceType = typeof(Resources.SharedResource))]
    public required string Password { get; set; }

    [DataType(DataType.Password)]
    [Required(ErrorMessageResourceType = typeof(Resources.SharedResource), ErrorMessageResourceName = "PasswordRequired")]
    [Display(Name = "ConfirmPassword", ResourceType = typeof(Resources.SharedResource))]
    [Compare("Password", ErrorMessageResourceType = typeof(Resources.SharedResource), ErrorMessageResourceName = "PasswordsDoNotMatch")]
    public required string ConfirmPassword { get; set; }
}