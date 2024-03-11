using System.Data;

namespace PriskollenServer.Library.Services;
public interface IDbContext
{
    IDbConnection CreateConnection();
}