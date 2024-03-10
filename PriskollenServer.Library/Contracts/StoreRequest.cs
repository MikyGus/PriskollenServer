namespace PriskollenServer.Library.Contracts;
public class StoreRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Image { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public int? StoreChain_id { get; set; }
}