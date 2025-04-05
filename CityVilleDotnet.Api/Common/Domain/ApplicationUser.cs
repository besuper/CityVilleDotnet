using Microsoft.AspNetCore.Identity;

namespace CityVilleDotnet.Api.Common.Domain;

public class ApplicationUser : IdentityUser
{
    public int Uid { get; set; }
}
