using TestExecutor.Models;

namespace TestExecutor.Services;

public interface IAccountsStore<T, Y, U, I>
{
    Task<String> LoginAsync(T login);
    Task<GetProfileViewModel> GetProfileAsync(String userId);
    Task<UpdateProfileViewModel> UpdateProfileAsync(U profile);
    Task<Boolean> ChangePasswordAsync(I changePassword);
}