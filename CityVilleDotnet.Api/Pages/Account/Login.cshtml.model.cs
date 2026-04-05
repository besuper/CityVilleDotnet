using System.ComponentModel.DataAnnotations;

namespace CityVilleDotnet.Api.Pages.Account;

public class LoginInputModel
{
    [Required(ErrorMessageResourceType = typeof(Resources.SharedResource), ErrorMessageResourceName = "UsernameRequired")]
    [Display(Name = "Username", ResourceType = typeof(Resources.SharedResource))]
    public required string Username { get; set; }

    [Required(ErrorMessageResourceType = typeof(Resources.SharedResource), ErrorMessageResourceName = "PasswordRequired")]
    [DataType(DataType.Password)]
    [Display(Name = "Password", ResourceType = typeof(Resources.SharedResource))]
    public required string Password { get; set; }

    [Display(Name = "RememberMe", ResourceType = typeof(Resources.SharedResource))]
    public bool RememberMe { get; set; }
}