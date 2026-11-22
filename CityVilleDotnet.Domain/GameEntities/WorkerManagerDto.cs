using System.Text.Json.Serialization;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.EnumExtensions;
using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Domain.GameEntities;

public class WorkerManagerDto<TAttributes, TData>
{
    [JsonPropertyName("attributes")] public required TAttributes Attributes { get; set; }

    [JsonPropertyName("members")] public required List<WorkerMemberDto<TData>> Members { get; set; }
}

public class WorkerMemberDto<TData>
{
    [JsonPropertyName("zid")] public required string Zid { get; set; }

    [JsonPropertyName("data")] public required TData Data { get; set; }
}

public class EmptyDto;

public class FactoryWorkerAttributesDto
{
    [JsonPropertyName("numPurchasedWorkers")] public int NumPurchasedWorkers { get; set; }

    [JsonPropertyName("contractName")] public required string ContractName { get; set; }
}

public class TrainWorkerAttributesDto
{
    [JsonPropertyName("trainName")] public required string TrainName { get; set; }

    [JsonPropertyName("operation")] public required string Operation { get; set; }

    [JsonPropertyName("commodityName")] public required string CommodityName { get; set; }

    [JsonPropertyName("timeSent")] public long TimeSent { get; set; }

    [JsonPropertyName("numPurchasedWorkers")] public int NumPurchasedWorkers { get; set; }
}

public class HunterWorkerDataDto
{
    [JsonPropertyName("state")] public required string State { get; set; }

    [JsonPropertyName("ts")] public long Ts { get; set; }
}

public static class WorkerManagerDtoMapper
{
    // load Global.factoryWorkerManager
    public static Dictionary<string, WorkerManagerDto<FactoryWorkerAttributesDto, EmptyDto>> ToFactoryWorkersDto(this World world)
    {
        return world.Objects
            .Where(x => x.ClassName == BuildingClassType.Factory && x.ContractName is not null)
            .ToDictionary(obj => $"w{obj.WorldFlatId}", obj => new WorkerManagerDto<FactoryWorkerAttributesDto, EmptyDto>
            {
                Attributes = new FactoryWorkerAttributesDto
                {
                    NumPurchasedWorkers = obj.CountPurchasedWorkers(),
                    ContractName = obj.ContractName!
                },
                Members = obj.Workers.Select(x => new WorkerMemberDto<EmptyDto> { Zid = x.Zid.ToString(), Data = new EmptyDto() }).ToList()
            });
    }

    // load Global.trainWorkerManager
    public static Dictionary<string, WorkerManagerDto<TrainWorkerAttributesDto, EmptyDto>> ToTrainWorkersDto(this World world)
    {
        var workers = new Dictionary<string, WorkerManagerDto<TrainWorkerAttributesDto, EmptyDto>>();

        if (world.TrainOrder is null) return workers;

        workers["w0"] = new WorkerManagerDto<TrainWorkerAttributesDto, EmptyDto>
        {
            Attributes = new TrainWorkerAttributesDto
            {
                TrainName = world.TrainOrder.ItemName,
                Operation = world.TrainOrder.Operation.ToDescriptionString(),
                CommodityName = world.TrainOrder.CommodityName,
                TimeSent = world.TrainOrder.TimeSent,
                NumPurchasedWorkers = world.TrainOrder.CountPurchasedStops()
            },
            Members = world.TrainOrder.Workers.Select(x => new WorkerMemberDto<EmptyDto> { Zid = x.Zid.ToString(), Data = new EmptyDto() }).ToList()
        };

        return workers;
    }

    // Every prey group reads its hunters from the "cops" bucket
    public static Dictionary<string, WorkerManagerDto<EmptyDto, HunterWorkerDataDto>> ToHunterWorkersDto(this PreyGroup? group)
    {
        return new Dictionary<string, WorkerManagerDto<EmptyDto, HunterWorkerDataDto>>
        {
            ["cops"] = new()
            {
                Attributes = new EmptyDto(),
                Members = group?.Hunters.OrderBy(x => x.Position).Select(x => new WorkerMemberDto<HunterWorkerDataDto>
                {
                    Zid = x.GetZid(),
                    Data = new HunterWorkerDataDto { State = x.State.ToDescriptionString(), Ts = x.Timestamp }
                }).ToList() ?? []
            }
        };
    }
}
