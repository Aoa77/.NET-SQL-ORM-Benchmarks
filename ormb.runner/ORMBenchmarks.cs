using BenchmarkDotNet.Attributes;
using ormb.core;

namespace ormb.runner;

[MemoryDiagnoser]
public abstract class ORMBenchmarks<T> where T : IUserRepository
{
    private IUserRepository? _repository;
    protected IUserRepository Repository => _repository!;

    protected abstract IUserRepositoryFactory<T> NewFactory();

    [GlobalSetup]
    public void Setup()
    {
        var factory = NewFactory();
        _repository = factory.CreateRepository();

        var users = Enumerable.Range(1, 1000)
            .Select(i => new User { Name = $"User{i}", Email = $"user{i}@example.com" })
            .ToList();
        _repository.BulkInsertAsync(users).GetAwaiter().GetResult();
    }

    [Benchmark]
    public async Task GetByIdAsync()
    {
        await Repository.GetByIdAsync(1);
    }

    [Benchmark]
    public async Task GetAllAsync()
    {
        await Repository.GetAllAsync();
    }

    [Benchmark]
    public async Task CreateAsync()
    {
        await Repository.CreateAsync(new User { Name = "NewUser", Email = "newuser@example.com" });
    }

    [Benchmark]
    public async Task UpdateAsync()
    {
        var user = await Repository.GetByIdAsync(1);
        user.Name = "UpdatedUser";
        await Repository.UpdateAsync(user);
    }

    [Benchmark]
    public async Task DeleteAsync()
    {
        await Repository.DeleteAsync(1);
    }

    [Benchmark]
    public async Task GetUsersByNameAsync()
    {
        await Repository.GetUsersByNameAsync("User");
    }

    [Benchmark]
    public async Task BulkInsertAsync()
    {
        var newUsers = Enumerable.Range(1001, 100)
            .Select(i => new User { Name = $"NewUser{i}", Email = $"newuser{i}@example.com" })
            .ToList();
        await Repository.BulkInsertAsync(newUsers);
    }
}
