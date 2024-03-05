namespace PriskollenServer.Library.Contracts;
public class StoreRequest
{
    public required string Name { get; set; }
    public string? Image { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public required int StoreChain_id { get; set; } = 0;
}