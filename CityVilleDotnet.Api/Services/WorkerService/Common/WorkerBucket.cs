namespace CityVilleDotnet.Api.Services.WorkerService.Common;

public static class WorkerBucket
{
    public const string FactoriesFeature = "factories";
    public const string TrainsFeature = "trains";
    public const string TrainsBucket = "w0";

    public static int ParseObjectId(string bucket)
    {
        if (bucket.Length < 2 || bucket[0] != 'w' || !int.TryParse(bucket.AsSpan(1), out var objectId))
            throw new Exception($"Invalid worker bucket {bucket}");

        return objectId;
    }
}
