using ormb.core;
using ormb.dapper;

namespace ormb.runner;

public class DapperBenchmarks : ORMBenchmarks<UserRepository>
{
    protected override IUserRepositoryFactory<UserRepository> NewFactory()
    {
        return new UserRepositoryFactory();
    }
}