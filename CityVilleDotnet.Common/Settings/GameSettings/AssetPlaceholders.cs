namespace CityVilleDotnet.Common.Settings.GameSettings;

public record AssetPlaceholder(string Path, int? Height);

public static class AssetPlaceholders
{
    private static readonly Dictionary<(int X, int Y), string> Constructions = new()
    {
        [(1, 1)] = "/assets/construction/scaffold_NE.png",
        [(2, 2)] = "/assets/construction/buildup2x2_2.png",
        [(3, 3)] = "/assets/construction/buildup3x3_2.png",
        [(4, 4)] = "/assets/construction/buildup4x4_2.png",
        [(5, 5)] = "/assets/construction/Buildup5x5_s2_NW.png",
        [(6, 6)] = "/assets/construction/buildup6x6_2.png",
        [(8, 8)] = "/assets/constructions/con_8x8/buildup8x8_2.png",
        [(10, 10)] = "/assets/constructions/con_10x10/buildup10x10_SW.png",
        [(3, 6)] = "/assets/construction/buildup3x6_2_SE.png",
        [(6, 3)] = "/assets/construction/buildup3x6_2_SW.png",
        [(4, 6)] = "/assets/construction/buildup4x6_2_SE.png",
        [(6, 4)] = "/assets/construction/buildup4x6_2_SW.png",
        [(4, 8)] = "/assets/construction/buildup4x8_2_SE.png",
        [(8, 4)] = "/assets/construction/buildup4x8_2_SW.png"
    };

    private static readonly Dictionary<(int X, int Y), string> Bases = new()
    {
        [(1, 1)] = "/assets/bases/base_tanconcrete/base_tanconcrete_1x1.png",
        [(2, 2)] = "/assets/bases/base_tanconcrete/base_tanconcrete_2x2.png",
        [(3, 3)] = "/assets/bases/base_tanconcrete/base_tanconcrete_3x3.png",
        [(4, 4)] = "/assets/bases/base_tanconcrete/base_tanconcrete_4x4.png",
        [(5, 5)] = "/assets/bases/base_tanconcrete/base_tanconcrete_5x5.png",
        [(6, 6)] = "/assets/bases/base_tanconcrete/base_tanconcrete_6x6.png",
        [(8, 8)] = "/assets/bases/base_tanconcrete/base_tanconcrete_8x8.png",
        [(12, 12)] = "/assets/bases/base_tanconcrete/base_tanconcrete_12x12.png",
        [(3, 6)] = "/assets/bases/base_tanconcrete/base_tanconcrete_3x6_SW.png",
        [(6, 3)] = "/assets/bases/base_tanconcrete/base_tanconcrete_3x6_SE.png",
        [(4, 6)] = "/assets/bases/base_tanconcrete/base_tanconcrete_4x6.png",
        [(6, 4)] = "/assets/bases/base_tanconcrete/base_tanconcrete_6x4.png",
        [(4, 8)] = "/assets/bases/base_tanconcrete/base_tanconcrete_4x8_SW.png",
        [(8, 4)] = "/assets/bases/base_tanconcrete/base_tanconcrete_4x8_SE.png",
        [(6, 12)] = "/assets/bases/base_tanconcrete/base_tanconcrete_6x12_SW.png",
        [(12, 6)] = "/assets/bases/base_tanconcrete/base_tanconcrete_6x12_SE.png"
    };

    public static IEnumerable<(string Url, AssetPlaceholder Placeholder)> FromStaticImage(ImageItem image, int sizeX, int sizeY)
    {
        var footprint = image.Direction is "SE" or "NW" ? (sizeY, sizeX) : (sizeX, sizeY);
        var construction = GetConstruction(footprint);

        if (image.Url is not null)
        {
            yield return ($"/{image.Url}", new AssetPlaceholder(construction, GetHeight(image, image)));
            yield break;
        }

        var building = image.Images.FirstOrDefault(x => x.Type == "building") ?? image.Images.FirstOrDefault(x => x.Type != "base");

        if (building?.Url is not null)
            yield return ($"/{building.Url}", new AssetPlaceholder(construction, GetHeight(image, building)));

        if (!Bases.TryGetValue(footprint, out var basePlaceholder)) yield break;

        foreach (var baseImage in image.Images.Where(x => x.Type == "base" && x.Url is not null))
        {
            yield return ($"/{baseImage.Url}", new AssetPlaceholder(basePlaceholder, GetHeight(image, baseImage)));
        }
    }

    private static int? GetHeight(ImageItem state, ImageItem layer)
    {
        var height = state.Height - layer.OffsetY;

        return state.Height > 0 && height > 0 ? height : null;
    }

    private static string GetConstruction((int X, int Y) footprint)
    {
        if (Constructions.TryGetValue(footprint, out var construction)) return construction;

        return Constructions
            .Where(x => x.Key.X == x.Key.Y && x.Key.X <= Math.Min(footprint.X, footprint.Y))
            .MaxBy(x => x.Key.X)
            .Value;
    }
}
