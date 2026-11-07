using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Settings;

namespace CityVilleDotnet.Domain.Entities;

public partial class World
{
    public string? PendingWorldName { get; private set; }

    public void SetPendingWorldName(string name)
    {
        PendingWorldName = name.Trim();
    }

    public string FinalizePendingWorldName()
    {
        var name = PendingWorldName ?? throw new DomainException(GameErrorType.InvalidState);

        PendingWorldName = null;

        return SetWorldName(name);
    }

    public bool CanRenameWithoutSignatures()
    {
        return Objects.Any(x => GameSettingsManager.Instance.GetItem(x.ItemName)?.Signatures == false);
    }
}
