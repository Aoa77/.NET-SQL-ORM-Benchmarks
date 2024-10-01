namespace ormb.core;

public interface IUserRepositoryFactory<T> where T : IUserRepository
{
    T CreateRepository();
}