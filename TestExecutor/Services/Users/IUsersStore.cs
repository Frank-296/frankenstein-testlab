using TestExecutor.Models;

namespace TestExecutor.Services;

public interface IUsersStore<T, Y, U>
{
    Task<IList<T>> GetUsersAsync(String id);
    Task<RegisterUserViewModel> AddUserAsync(Y registerUser);
    Task<T> GetUserAsync(String id);
    Task<UpdateUserViewModel> UpdateUserAsync(U updateUser);
    Task<Boolean> ResetUserPasswordAsync(String id);
    Task<Boolean> DeleteUserAsync(String id);
}