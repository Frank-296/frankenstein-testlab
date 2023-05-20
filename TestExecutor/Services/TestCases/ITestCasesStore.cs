using Microsoft.AspNetCore.Components;

using TestExecutor.Models;

namespace TestExecutor.Services;

public interface ITestCasesStore<T>
{
    Task<IList<T>> GetTestCasesAsync(String businessProcessId);
    Task<TestCase> AddTestCaseAsync(T testCase, String testApplicationId, NavigationManager navigationManager);
    Task<T> GetTestCaseAsync(String testCaseId);
    Task<TestCase> UpdateTestCaseAsync(T testCase, String testApplicationId, NavigationManager navigationManager);
    Task<Boolean> DeleteTestCaseAsync(String testCaseId);
}