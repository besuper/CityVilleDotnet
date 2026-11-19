namespace CityVilleDotnet.Domain.Entities;

public partial class Player
{
    public List<int> ConsumeWeatherItem()
    {
        RemoveItem("material_weather_umbrellas"); // Change this if implement runtimeData.active_weather

        // Change pnj name is cherry rain
        var secureRands = CollectDoobersRewards("NPC_rainman_1", modifiersName: "rainHelperTable", energyActionPack: false);

        HandleQuestsProgress("giveWeatherItem", className: "showers");

        return secureRands;
    }
}
