namespace PriskollenServer.Library.Contracts;
public record ProductResponse(
    Guid Id,
    string Name,
    string Image,
    string BarCode,
    DateTime Created,
    DateTime Modified);