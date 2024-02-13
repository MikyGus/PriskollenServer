namespace PriskollenServer.Library.Contracts;
public record StoreRequest(
    string Name,
    string Coordinates,
    string Address,
    string City,
    Guid StoreChain);