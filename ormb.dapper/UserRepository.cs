using System.Data;
using Dapper;
using ormb.core;

namespace ormb.dapper;

public sealed class UserRepository : IUserRepository
{
    private readonly IDbConnection _connection;

    public UserRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<User> GetByIdAsync(int id)
    {
        var user = await _connection.QuerySingleOrDefaultAsync<User>(
            "SELECT * FROM Users WHERE Id = @Id", new { Id = id });

        if (user == null)
        {
            throw new KeyNotFoundException($"User with Id {id} not found.");
        }
        return user;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _connection.QueryAsync<User>("SELECT * FROM Users");
    }

    public async Task<int> CreateAsync(User user)
    {
        var sql = "INSERT INTO Users (Name, Email) VALUES (@Name, @Email); SELECT CAST(SCOPE_IDENTITY() as int)";
        return await _connection.ExecuteScalarAsync<int>(sql, user);
    }

    public async Task UpdateAsync(User user)
    {
        await _connection.ExecuteAsync("UPDATE Users SET Name = @Name, Email = @Email WHERE Id = @Id", user);
    }

    public async Task DeleteAsync(int id)
    {
        await _connection.ExecuteAsync("DELETE FROM Users WHERE Id = @Id", new { Id = id });
    }

    public async Task<IEnumerable<User>> GetUsersByNameAsync(string name)
    {
        return await _connection.QueryAsync<User>("SELECT * FROM Users WHERE Name LIKE @Name", new { Name = $"%{name}%" });
    }

    public async Task<int> BulkInsertAsync(IEnumerable<User> users)
    {
        return await _connection.ExecuteAsync("INSERT INTO Users (Name, Email) VALUES (@Name, @Email)", users);
    }
}
