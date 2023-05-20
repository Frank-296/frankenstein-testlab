using Microsoft.AspNetCore.Components;

using TestExecutor.Models;
using TestLab.Utilities;

namespace TestExecutor.Services;

public interface IExecutionsStore<T>
{
    Task<IList<T>> GetExecutionsAsync(String testCaseId, String userId, ExecutionStatus? executionStatus);
    Task<Execution> AddExecutionAsync(T execution, String businessProcessId, String testApplicationId, NavigationManager navigationManager, String from, Boolean showResult);
    Task<T> GetExecutionAsync(String executionId, Boolean loadTestReport, Boolean loadTestData);
    Task<Execution> UpdateExecutionAsync(T execution, String businessProcessId, String testApplicationId, NavigationManager navigationManager, String from, Boolean showResult);
    Task<Boolean> DeleteExecutionAsync(String executionId);
}