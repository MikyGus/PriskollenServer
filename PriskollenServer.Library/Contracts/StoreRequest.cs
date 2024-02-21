namespace PriskollenServer.Library.Contracts;
public record StoreRequest(
    string Name,
    string Image,
    double Latitude,
    double Longitude,
    string Address,
    string City,
    int StoreChain_id);