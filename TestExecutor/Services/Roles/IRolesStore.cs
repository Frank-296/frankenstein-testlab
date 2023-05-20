using TestExecutor.Models;

namespace TestExecutor.Services;

public interface IRolesStore<T>
{
    Task<IList<T>> GetRolesAsync();
    Task<Role> AddRoleAsync(T role);
    Task<T> GetRoleAsync(String roleId);
    Task<Role> UpdateRoleAsync(T role);
    Task<Boolean> DeleteRoleAsync(String roleId);
}