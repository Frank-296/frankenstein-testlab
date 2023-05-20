using TestExecutor.Models;

namespace TestExecutor.Services;

public interface ICompaniesStore<T>
{
    Task<IList<T>> GetCompaniesAsync();
    Task<Company> AddCompanyAsync(T company);
    Task<T> GetCompanyAsync(String companyId);
    Task<Company> UpdateCompanyAsync(T company);
    Task<Boolean> DeleteCompanyAsync(String companyId);
}