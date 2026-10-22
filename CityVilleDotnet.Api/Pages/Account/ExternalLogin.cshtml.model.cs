using System.ComponentModel.DataAnnotations;

namespace CityVilleDotnet.Api.Pages.Account;

public class ExternalLoginInputModel
{
    [Required(ErrorMessageResourceType = typeof(Resources.SharedResource), ErrorMessageResourceName = "UsernameRequired")]
    [MinLength(4, ErrorMessageResourceType = typeof(Resources.SharedResource), ErrorMessageResourceName = "UsernameMinLength")]
    [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessageResourceType = typeof(Resources.SharedResource), ErrorMessageResourceName = "UsernameInvalidChars")]
    [Display(Name = "Username", ResourceType = typeof(Resources.SharedResource))]
    public required string Username { get; set; }
}