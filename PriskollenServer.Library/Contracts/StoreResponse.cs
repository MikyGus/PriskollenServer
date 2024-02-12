namespace PriskollenServer.Library.Contracts;
public record StoreResponse(
    Guid Id,
    string Name,
    string Coordinates,
    string Address,
    string City,
    Guid StoreChain,
    DateTime Created,
    DateTime Modified);