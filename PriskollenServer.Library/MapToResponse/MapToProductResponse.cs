using PriskollenServer.Library.Contracts;
using PriskollenServer.Library.Models;

namespace PriskollenServer.Library.MapToResponse;
public class MapToProductResponse : IMapToResponse<Product, ProductResponse>
{
    public IEnumerable<ProductResponse> MapToResponse(IEnumerable<Product> models)
    {
        foreach (Product model in models)
        {
            yield return MapToResponse(model);
        }
    }

    public ProductResponse MapToResponse(Product model)
        => new()
        {
            Id = model.Id,
            Barcode = model.Barcode,
            Name = model.Name,
            Brand = model.Brand,
            Image = model.Image,
            Volume = model.Volume,
            VolumeWithLiquid = model.VolumeWithLiquid,
            VolumeUnit = model.VolumeUnit,
            Created = model.Created,
            Modified = model.Modified
        };
}