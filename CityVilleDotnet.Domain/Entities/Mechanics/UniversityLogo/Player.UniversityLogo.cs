using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Settings.GameSettings;

namespace CityVilleDotnet.Domain.Entities;

public partial class Player
{
    public List<SubLicense> SubLicenses { get; private set; } = [];

    public bool HasSubLicense(GameItem spawner, string logoName)
    {
        return SubLicenses.FirstOrDefault(x => x.Name == logoName)?.Has(spawner.SubLicenseId) ?? false;
    }

    // From UniversityLogoCell::doPurchase
    public void AcquireSubLicense(GameItem spawner, GameItem logo)
    {
        if (spawner.GetGameEventMechanic("universityLogo")?.AllowsItem(logo) != true)
        {
            throw new DomainException(GameErrorType.InvalidData, $"{logo.Name} can't be licensed for {spawner.Name}");
        }

        if (logo.UnlockCost is not > 0)
        {
            throw new DomainException(GameErrorType.InvalidData, $"{logo.Name} doesn't need a license");
        }

        if (HasSubLicense(spawner, logo.Name))
        {
            throw new DomainException(GameErrorType.InvalidState, $"{logo.Name} is already licensed for {spawner.Name}");
        }

        RemoveCash(logo.UnlockCost.Value);

        var subLicense = SubLicenses.FirstOrDefault(x => x.Name == logo.Name);

        if (subLicense is null)
        {
            subLicense = new SubLicense(logo.Name);
            SubLicenses.Add(subLicense);
        }

        subLicense.Add(spawner.SubLicenseId);
    }

    // From SubLicensedPropertyMechanic::validatePropertyName
    public void SetUniversityLogo(WorldObject obj, GameItem spawner, MechanicItem mechanic, GameItem? logo)
    {
        if (logo is not null)
        {
            if (!mechanic.AllowsItem(logo))
            {
                throw new DomainException(GameErrorType.InvalidData, $"{logo.Name} is not allowed on {spawner.Name}");
            }

            if (logo.UnlockCost > 0 && !HasSubLicense(spawner, logo.Name))
            {
                throw new DomainException(GameErrorType.InvalidState, $"{logo.Name} is not licensed for {spawner.Name}");
            }
        }

        obj.SetUniversityLogo(logo?.Name);
    }
}
