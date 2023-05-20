using Microsoft.AspNetCore.Components;

using TestExecutor.Models;

namespace TestExecutor.Services;

public interface ISuiteExecutionsStore<T>
{
    Task<IList<T>> GetSuiteExecutionsAsync(String businessProcessId, String userId);
    Task<SuiteExecution> AddSuiteExecutionAsync(T suiteExecution, String businessProcessId, String testApplicationId, NavigationManager navigationManager);
    Task<T> GetSuiteExecutionAsync(String suiteExecutionId, Boolean loadTestCases);
	Task<SuiteExecution> UpdateSuiteExecutionAsync(T suiteExecution, String businessProcessId, String testApplicationId, NavigationManager navigationManager);
    Task<Boolean> DeleteSuiteExecutionAsync(String suiteExecutionId);
}