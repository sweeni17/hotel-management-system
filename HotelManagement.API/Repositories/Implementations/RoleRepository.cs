using HotelManagement.API.Data;
using HotelManagement.API.Models;
using HotelManagement.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.API.Repositories.Implementations;

public class RoleRepository : IRoleRepository
{
    private readonly HotelDbContext _dbContext;

    public RoleRepository(HotelDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Role>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Roles
            .AsNoTracking()
            .OrderBy(role => role.RoleName)
            .ToListAsync(cancellationToken);
    }

    public async Task<Role?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Roles
            .FirstOrDefaultAsync(role => role.RoleId == id, cancellationToken);
    }

    public async Task<Role> AddAsync(Role entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Roles.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<Role?> UpdateAsync(Role entity, CancellationToken cancellationToken = default)
    {
        var existingRole = await _dbContext.Roles
            .FirstOrDefaultAsync(role => role.RoleId == entity.RoleId, cancellationToken);

        if (existingRole is null)
        {
            return null;
        }

        existingRole.RoleName = entity.RoleName;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return existingRole;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var role = await _dbContext.Roles
            .FirstOrDefaultAsync(role => role.RoleId == id, cancellationToken);

        if (role is null)
        {
            return false;
        }

        _dbContext.Roles.Remove(role);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Roles
            .AnyAsync(role => role.RoleId == id, cancellationToken);
    }

    public async Task<Role?> GetByNameAsync(string roleName, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Roles
            .FirstOrDefaultAsync(role => role.RoleName == roleName, cancellationToken);
    }
}
