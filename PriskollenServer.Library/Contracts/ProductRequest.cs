namespace PriskollenServer.Library.Contracts;
public record ProductRequest(
    string Name,
    string Image,
    string BarCode);