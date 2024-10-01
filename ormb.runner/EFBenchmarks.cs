using ormb.core;
using ormb.ef;

namespace ormb.runner;

public class EFBenchmarks : ORMBenchmarks<UserRepository>
{
    protected override IUserRepositoryFactory<UserRepository> NewFactory()
    {
        return new UserRepositoryFactory();
    }
}