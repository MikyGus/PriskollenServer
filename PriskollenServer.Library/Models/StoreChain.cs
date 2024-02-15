using ErrorOr;
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
    {
        List<Error> errors = [];
        if (name.Length is < MinNameLength or > MaxNameLength)
        {
            errors.Add(Errors.StoreChain.InvalidName);
        }
        if (errors.Count > 0)
        {
            return errors;
        }

        return new StoreChain(id ?? Guid.NewGuid(), name, image, created ?? DateTime.UtcNow, DateTime.UtcNow);
    }
}