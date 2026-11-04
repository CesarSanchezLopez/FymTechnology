using FymTechnology.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FymTechnology.Infrastructure.Persistence;

public class FymDbContext : DbContext
{
    public FymDbContext(DbContextOptions<FymDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<UserRole> UserRoles => Set<UserRole>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(
            typeof(FymDbContext).Assembly);
    }
}