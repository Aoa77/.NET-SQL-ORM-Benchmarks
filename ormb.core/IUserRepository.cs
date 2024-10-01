namespace ormb.core;

public interface IUserRepository
{
    Task<User> GetByIdAsync(int id);
    Task<IEnumerable<User>> GetAllAsync();
    Task<int> CreateAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(int id);
    Task<IEnumerable<User>> GetUsersByNameAsync(string name);
    Task<int> BulkInsertAsync(IEnumerable<User> users);
}