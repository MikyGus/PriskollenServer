namespace PriskollenServer.Library.Contracts;
public class ProductRequest
{
    public string Barcode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public decimal Volume { get; set; }
    public decimal? VolumeWithLiquid { get; set; } // If applicable, if not this is null
    public string VolumeUnit { get; set; } = string.Empty; // gr or litre
}