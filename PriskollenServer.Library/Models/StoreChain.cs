namespace PriskollenServer.Library.Models;
public class StoreChain(int id, string name, string image, DateTime created, DateTime modified)
{
    public int Id { get; init; } = id;
    public string Name { get; init; } = name;
    public string Image { get; init; } = image;
    public DateTime Created { get; init; } = created;
    public DateTime Modified { get; init; } = modified;
}