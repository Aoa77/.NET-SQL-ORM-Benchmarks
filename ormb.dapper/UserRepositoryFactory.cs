using Microsoft.Data.Sqlite;
using ormb.core;
using System.Data;

namespace ormb.dapper;

public sealed class UserRepositoryFactory : IUserRepositoryFactory<UserRepository>
{
    public UserRepository CreateRepository()
    {
        var connection = CreateConnection();
        var repository = new UserRepository(connection);
        return repository;
    }

    private static IDbConnection CreateConnection()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        // Create the table(s) needed for your tests
        using (var command = connection.CreateCommand())
        {
            command.CommandText = @"
                CREATE TABLE Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Email TEXT NOT NULL
                )";
            command.ExecuteNonQuery();
        }

        return connection;
    }
}