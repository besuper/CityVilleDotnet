namespace CityVilleDotnet.Domain.Enums;

public enum GameErrorType
{
    NoError = 0,
    ErrorAuth = 1,
    OutdatedGameVersion = 10,
    AuthNoUserId = 26,
    AuthNoSession = 27,
    RetryTransaction = 28,
    ForceReload = 29,
    UserDataMissing = 2,
    InvalidState = 3,
    InvalidData = 4,
    MissingData = 5,
    ActionClassError = 6,
    ActionMethodError = 7,
    ResourceDataMissing = 8,
    NotEnoughMoney = 9,
    TransportFailureGeneral = 25
}