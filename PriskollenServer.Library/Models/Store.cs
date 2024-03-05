namespace PriskollenServer.Library.Models;
public class Store
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Image { get; init; } = string.Empty;
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public double? Distance { get; init; } = null;
    public string Address { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public int Storechain_id { get; init; }
    public DateTime Created { get; init; }
    public DateTime Modified { get; init; }
}