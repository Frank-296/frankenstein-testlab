using TestExecutor.Models;

namespace TestExecutor.Services;

public interface IBusinessProcessesStore<T>
{
    Task<IList<T>> GetBusinessProcessesAsync(String testApplicationId);
    Task<BusinessProcess> AddBusinessProcessAsync(T businessProcess);
    Task<T> GetBusinessProcessAsync(String businessProcessId);
    Task<BusinessProcess> UpdateBusinessProcessAsync(T businessProcess);
    Task<Boolean> DeleteBusinessProcessAsync(String businessProcessId);
}