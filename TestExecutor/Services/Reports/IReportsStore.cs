namespace TestExecutor.Services;

public interface IReportsStore<T>
{
    Task<IList<T>> GenerateReportAsync(DateTime startDate, DateTime endDate, String userId);
}