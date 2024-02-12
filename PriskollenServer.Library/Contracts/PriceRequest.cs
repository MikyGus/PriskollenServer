namespace PriskollenServer.Library.Contracts;
public record PriceRequest(
    Guid StoreID,
    Guid ProductID,
    Decimal Price);