namespace PriskollenServer.Library.Contracts;
public record StoreChainResponse(
    Guid Id,
    string Name,
    string Image,
    DateTime Created,
    DateTime Modified);