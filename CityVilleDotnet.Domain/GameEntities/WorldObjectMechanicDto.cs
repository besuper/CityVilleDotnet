using System.Text.Json.Serialization;

namespace CityVilleDotnet.Domain.GameEntities;

public class MacroObjectDataDto
{
    [JsonPropertyName("parentMacroObjectId")] public required string ParentMacroObjectId { get; set; }

    [JsonPropertyName("isCandidate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? IsCandidate { get; set; }
}

public class InvestmentDataDto
{
    [JsonPropertyName("coin")] public int Coin { get; set; }

    [JsonPropertyName("coin_numInvestments")] public int CoinNumInvestments { get; set; }
}

public class RemodelDto
{
    [JsonPropertyName("itemName")] public required string ItemName { get; set; }

    [JsonPropertyName("gate")] public required RemodelGateDto Gate { get; set; }

    [JsonPropertyName("complete")] public bool Complete { get; set; }
}

public class RemodelGateDto
{
    [JsonPropertyName("builds")] public int Builds { get; set; }
}

public class ContractDataDto
{
    [JsonPropertyName("contractName")] public required string ContractName { get; set; }

    [JsonPropertyName("plantTime")] public double PlantTime { get; set; }
}

public class StreakDataDto
{
    [JsonPropertyName("activationTime")] public long ActivationTime { get; set; }

    [JsonPropertyName("inactiveTime")] public long InactiveTime { get; set; }

    [JsonPropertyName("streakLength")] public int StreakLength { get; set; }
}

public class CustomerHarvestStateDto
{
    [JsonPropertyName("customers")] public int Customers { get; set; }

    [JsonPropertyName("customersReq")] public int CustomersReq { get; set; }

    [JsonPropertyName("resources")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? Resources { get; set; }

    [JsonPropertyName("filledSlotCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? FilledSlotCount { get; set; }
}

public class TimedHarvestStateDto
{
    [JsonPropertyName("state")] public required string State { get; set; }

    [JsonPropertyName("timestamp")] public long Timestamp { get; set; }

    [JsonPropertyName("endTimestamp")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? EndTimestamp { get; set; }
}

public class HarvestLoopStateDto
{
    [JsonPropertyName("openTS")] public long OpenTs { get; set; }

    [JsonPropertyName("closeTS")] public long CloseTs { get; set; }
}

public class HarvestLoopDataDto
{
    [JsonPropertyName("startTS")] public long StartTs { get; set; }
}

public class WonderPeepDto
{
    [JsonPropertyName("coupon")] public required string Coupon { get; set; }
}

public class SleepStateDto
{
    [JsonPropertyName("sleepTS")] public long SleepTs { get; set; }

    [JsonPropertyName("wakeTS")] public long WakeTs { get; set; }
}

public class GreenHouseStorageDto
{
    [JsonPropertyName("item")] public required WorldObjectDto Item { get; set; }
}

public class GateDto
{
    [JsonPropertyName("name")] public string? Name { get; set; }

    [JsonPropertyName("type")] public string? Type { get; set; }

    [JsonPropertyName("keys")] public required Dictionary<string, int> Keys { get; set; }
}
