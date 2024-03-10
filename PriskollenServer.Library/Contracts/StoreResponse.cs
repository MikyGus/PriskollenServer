namespace PriskollenServer.Library.Contracts;
public record StoreResponse(
    int Id,
    string Name,
    string Image,
    double Latitude,
    double Longitude,
    string Address,
    string City,
    StoreChainResponse StoreChain,
    DateTime Created,
    DateTime Modified,
    double? Distance = null);