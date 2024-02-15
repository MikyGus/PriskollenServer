using ErrorOr;
using PriskollenServer.Library.Contracts;
using PriskollenServer.Library.ServiceErrors;

namespace PriskollenServer.Library.Models;
public class StoreChain
{
    public const int MinNameLength = 3;
    public const int MaxNameLength = 30;

    public Guid Id { get; }
    public string Name { get; }
    public string Image { get; } = string.Empty;
    public DateTime Created { get; }
    public DateTime Modified { get; }

    private StoreChain(Guid id, string name, string image, DateTime created, DateTime modified)
    {
        Id = id;
        Name = name;
        Image = image;
        Created = created;
        Modified = modified;
    }

    public static ErrorOr<StoreChain> Create(string name, string image, Guid? id = null, DateTime? created = null)
        => Isvalid(new StoreChainRequest(name, image), out List<Error> errors)
            ? new StoreChain(id ?? Guid.NewGuid(), name, image, created ?? DateTime.UtcNow, DateTime.UtcNow)
            : errors;

    public static ErrorOr<StoreChain> CreateFrom(StoreChainRequest request)
        => Isvalid(request, out List<Error> errors)
            ? new StoreChain(Guid.NewGuid(), request.Name, request.Image, DateTime.UtcNow, DateTime.UtcNow)
            : errors;
    public static ErrorOr<StoreChain> CreateFrom(Guid id, StoreChainRequest request)
        => Isvalid(request, out List<Error> errors)
            ? new StoreChain(id, request.Name, request.Image, DateTime.UtcNow, DateTime.UtcNow)
            : errors;

    private static bool Isvalid(StoreChainRequest request, out List<Error> errors)
    {
        errors = [];
        if (request.Name.Length is < MinNameLength or > MaxNameLength)
        {
            errors.Add(Errors.StoreChain.InvalidName);
        }
        return errors.Count == 0;
    }
}