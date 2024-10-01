using Microsoft.EntityFrameworkCore;
using ormb.core;

namespace ormb.ef;

public sealed class UserRepositoryFactory : IUserRepositoryFactory<UserRepository>
{
    public UserRepository CreateRepository()
    {
        var options = new DbContextOptionsBuilder<UserDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        var context = new UserDbContext(options);
        var repository = new UserRepository(context);

        return repository;
    }
}
