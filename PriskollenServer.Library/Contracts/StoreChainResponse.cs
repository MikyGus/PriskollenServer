namespace PriskollenServer.Library.Contracts;
public record StoreChainResponse(
    int Id,
    string Name,
    string Image,
    DateTime Created,
    DateTime Modified);