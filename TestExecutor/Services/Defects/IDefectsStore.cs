using Microsoft.AspNetCore.Components;

using TestExecutor.Models;

namespace TestExecutor.Services;

public interface IDefectsStore<T>
{
    Task<IList<T>> GetDefectsAsync(String userId);
    Task<Defect> AddDefectAsync(T defect, NavigationManager navigationManager);
    Task<T> GetDefectAsync(String executionId);
    Task<Defect> UpdateDefectAsync(T defect);
    Task<Boolean> DeleteDefectAsync(String defectId);
}