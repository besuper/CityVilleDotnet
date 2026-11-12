using CityVilleDotnet.Common.Settings;

namespace CityVilleDotnet.Domain.Entities;

public partial class World
{
    private static readonly (string ItemName, double Discount)[] CargoTerminalDiscounts =
    [
        ("macro_terminal_cargo_2", 0.08),
        ("macro_terminal_cargo_3", 0.15),
        ("macro_terminal_cargo_4", 0.25),
        ("macro_terminal_cargo_5", 0.35)
    ];

    // Item::applyDiscountGrowTime, the first upgraded cargo terminal found sets the discount
    public double GetAirplaneGrowTimeDiscount()
    {
        return CargoTerminalDiscounts.FirstOrDefault(x => CountBuildingByName(x.ItemName) > 0).Discount;
    }

    // EditMacroObjectCandidateMechanic::placeCandidate
    public void PlaceMacroObjectCandidate(WorldObject candidate, string placementValidator)
    {
        candidate.DetachFromMacroObject();

        var anchor = FindPlacementAnchor(candidate, placementValidator);

        if (anchor?.ParentMacroObjectId is null) return;

        candidate.AttachToMacroObjectAsCandidate(anchor.ParentMacroObjectId);
    }

    // PlaceVerification::airplaneVerification / terminalVerification
    private WorldObject? FindPlacementAnchor(WorldObject candidate, string placementValidator)
    {
        var candidateItem = GameSettingsManager.Instance.GetItem(candidate.ItemName) ?? throw new Exception($"Can't find game item for {candidate.ItemName}");
        var (width, height) = candidate.GetSize(candidateItem);

        foreach (var obj in Objects.OrderBy(x => x.WorldFlatId))
        {
            var item = GameSettingsManager.Instance.GetItem(obj.ItemName);

            if (item is null) continue;

            var (objWidth, _) = obj.GetSize(item);

            var isAnchor = placementValidator switch
            {
                "airplaneVerification" => item.HasKeyword("airport") && obj.X <= candidate.X && candidate.X + width <= obj.X + objWidth && candidate.Y + height == obj.Y,
                "terminalVerification" => item.HasKeyword("airportExtendable") && (obj.X + objWidth == candidate.X || obj.X == candidate.X + width) && candidate.Y == obj.Y,
                _ => throw new Exception($"Unsupported placement validator {placementValidator}")
            };

            if (isAnchor) return obj;
        }

        return null;
    }
}
