using CityVilleDotnet.Common.Enums;

namespace CityVilleDotnet.Common.Exceptions;

public class DomainException : Exception
{
    public GameErrorType Reason { get; private set; }

    public DomainException()
    {
    }
    
    public DomainException(GameErrorType reason)
    {
        Reason = reason;
    }
}