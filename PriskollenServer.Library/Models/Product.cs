namespace PriskollenServer.Library.Models;
public class Product
{
    public int Id { get; set; }
    public string Barcode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public double Volume { get; set; }
    public double? VolumeWithLiquid { get; set; } // If applicable, if not this is null
    public string VolumeUnit { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
}