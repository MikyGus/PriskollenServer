namespace PriskollenServer.Library.Contracts;
public record PriceResponse(
    Guid Id,
    Guid StoreID,
    Guid ProductID,
    Decimal Price);