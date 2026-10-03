using CityVilleDotnet.Common.Enums;

namespace CityVilleDotnet.Common.Exceptions;

public class DomainException : Exception
{
    public GameErrorType Reason { get; private set; }
    public string ClientMessage { get; private set; } = string.Empty;

    public DomainException()
    {
    }
    
    public DomainException(GameErrorType reason)
    {
        Reason = reason;
    }
    
    public DomainException(GameErrorType reason, string message)
    {
        Reason = reason;
        ClientMessage = message;
    }
}