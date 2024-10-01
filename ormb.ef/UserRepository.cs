using Microsoft.EntityFrameworkCore;
using ormb.core;

namespace ormb.ef;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _context;

    public UserRepository(UserDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetByIdAsync(int id)
    {
        var user = await _context.Set<User>().FindAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {id} not found.");
        }
        return user;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Set<User>().ToListAsync();
    }

    public async Task<int> CreateAsync(User user)
    {
        await _context.Set<User>().AddAsync(user);
        await _context.SaveChangesAsync();
        return user.Id;
    }

    public async Task UpdateAsync(User user)
    {
        _context.Set<User>().Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var user = await GetByIdAsync(id);
        _context.Set<User>().Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<User>> GetUsersByNameAsync(string name)
    {
        return await _context.Set<User>()
            .Where(u => u.Name!.Contains(name))
            .ToListAsync();
    }

    public async Task<int> BulkInsertAsync(IEnumerable<User> users)
    {
        await _context.Set<User>().AddRangeAsync(users);
        return await _context.SaveChangesAsync();
    }
}
