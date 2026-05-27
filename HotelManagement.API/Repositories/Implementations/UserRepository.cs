using HotelManagement.API.Data;
using HotelManagement.API.Models;
using HotelManagement.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.API.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private readonly HotelDbContext _dbContext;

    public UserRepository(HotelDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .Include(user => user.Role)
            .OrderBy(user => user.FullName)
            .ToListAsync(cancellationToken);
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(user => user.UserId == id, cancellationToken);
    }

    public async Task<User> AddAsync(User entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt ??= DateTime.UtcNow;

        await _dbContext.Users.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<User?> UpdateAsync(User entity, CancellationToken cancellationToken = default)
    {
        var existingUser = await _dbContext.Users
            .FirstOrDefaultAsync(user => user.UserId == entity.UserId, cancellationToken);

        if (existingUser is null)
        {
            return null;
        }

        existingUser.FullName = entity.FullName;
        existingUser.Email = entity.Email;
        existingUser.Phone = entity.Phone;
        existingUser.RoleId = entity.RoleId;

        if (!string.IsNullOrWhiteSpace(entity.PasswordHash))
        {
            existingUser.PasswordHash = entity.PasswordHash;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return existingUser;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(user => user.UserId == id, cancellationToken);

        if (user is null)
        {
            return false;
        }

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .AnyAsync(user => user.UserId == id, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
    }

    public async Task<User?> GetByEmailWithRoleAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .Include(user => user.Role)
            .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
    }

    public async Task<User?> GetByIdWithRoleAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .Include(user => user.Role)
            .FirstOrDefaultAsync(user => user.UserId == id, cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .AnyAsync(user => user.Email == email, cancellationToken);
    }
}
