namespace PriskollenServer.Library.Models;
public record Store(
    int Id,
    string Name,
    string Image,
    double Latitude,
    double Longitude,
    string Address,
    string City,
    StoreChain StoreChain,
    DateTime Created,
    DateTime Modified);