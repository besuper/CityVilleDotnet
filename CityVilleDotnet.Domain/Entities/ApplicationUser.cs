using Microsoft.AspNetCore.Identity;

namespace CityVilleDotnet.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public bool IsGuest { get; set; } = false;
    public DateTime CreationDate { get; set; } = DateTime.Now;
}
