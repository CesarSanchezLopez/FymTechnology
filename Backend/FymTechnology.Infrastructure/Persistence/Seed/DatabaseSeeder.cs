using FymTechnology.Application.Interfaces;
using FymTechnology.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FymTechnology.Infrastructure.Persistence.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(
        IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        using var scope = serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<FymDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        var roleNames = new[] { "SuperAdmin", "Admin", "User" };

        foreach (var roleName in roleNames)
        {
            if (!await context.Roles.AnyAsync(x => x.Name == roleName))
            {
                context.Roles.Add(new Role
                {
                    Id = Guid.NewGuid(),
                    Name = roleName,
                    Description = $"{roleName} role"
                });
            }
        }

        await context.SaveChangesAsync();

        var email = configuration["InitialSuperAdmin:Email"];
        var password = configuration["InitialSuperAdmin:Password"];

        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException(
                "Initial SuperAdmin credentials are not configured.");
        }

        var normalizedEmail = email.Trim().ToLowerInvariant();

        var user = await context.Users
            .FirstOrDefaultAsync(x => x.Email == normalizedEmail);

        if (user is null)
        {
            user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Super",
                LastName = "Administrator",
                Email = normalizedEmail,
                PasswordHash = passwordHasher.Hash(password),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        var superAdminRole = await context.Roles
            .FirstAsync(x => x.Name == "SuperAdmin");

        var assignmentExists = await context.UserRoles.AnyAsync(
            x => x.UserId == user.Id && x.RoleId == superAdminRole.Id);

        if (!assignmentExists)
        {
            context.UserRoles.Add(new UserRole
            {
                UserId = user.Id,
                RoleId = superAdminRole.Id,
                AssignedAt = DateTime.UtcNow
            });

            await context.SaveChangesAsync();
        }
    }
}