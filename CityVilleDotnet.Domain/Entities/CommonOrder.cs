using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Domain.Entities;

public abstract class CommonOrder
{
    public int Id { get; set; }
    public required string SenderId { get; set; }
    public required string RecipientId { get; set; }
    public long TimeSent { get; set; } 
    public long LastTimeReminded { get; set; }
    public OrderType OrderType { get; set; }
    public OrderState OrderState { get; set; }
    public TransmissionStatus TransmissionStatus { get; set; }
    
    public void Accept()
    {
        OrderState = OrderState.Accepted;
    }

    public void Deny()
    {
        OrderState = OrderState.Denied;
    }
}