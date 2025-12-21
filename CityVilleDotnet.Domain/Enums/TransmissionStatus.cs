using System.ComponentModel;

namespace CityVilleDotnet.Domain.Enums;

public enum TransmissionStatus
{
    [Description("sent")] Sent = 0,
    [Description("received")] Received = 1
}