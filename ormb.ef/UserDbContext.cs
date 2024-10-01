using Microsoft.EntityFrameworkCore;
using ormb.core;

namespace ormb.ef;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
}
