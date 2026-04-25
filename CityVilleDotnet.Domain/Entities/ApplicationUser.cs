using Microsoft.AspNetCore.Identity;

namespace CityVilleDotnet.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public DateTime CreationDate { get; set; } = DateTime.Now;
}
