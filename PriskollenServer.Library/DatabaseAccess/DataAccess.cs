using Dapper;
using MySql.Data.MySqlClient;
using System.Data;

namespace PriskollenServer.Library.DatabaseAccess;
public class DataAccess : IDataAccess
{
    public async Task<List<T>> LoadData<T, U>(string sql, U parameters, string connectionString)
    {
        using IDbConnection connection = new MySqlConnection(connectionString);
        IEnumerable<T> rows = await connection.QueryAsync<T>(sql, parameters);
        return rows.ToList();
    }

    public Task SaveData<T>(string sql, T parameters, string connectionString)
    {
        using IDbConnection connection = new MySqlConnection(connectionString);
        return connection.ExecuteAsync(sql, parameters);
    }
}