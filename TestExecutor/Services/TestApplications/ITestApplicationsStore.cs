using TestExecutor.Models;

namespace TestExecutor.Services;

public interface ITestApplicationsStore<T>
{
    Task<IList<T>> GetTestApplicationsAsync();
    Task<TestApplication> AddTestApplicationAsync(T testApplication);
    Task<T> GetTestApplicationAsync(String testApplicationId);
    Task<TestApplication> UpdateTestApplicationAsync(T testApplication);
    Task<Boolean> DeleteTestApplicationAsync(String testApplicationId);
}