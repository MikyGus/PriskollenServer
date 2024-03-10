namespace PriskollenServer.Library.Contracts;
public record GpsRequest(
    double? Latitude,
    double? Longitude)
{
    public bool PositionIsProvided => (Latitude != null && Longitude != null);
}