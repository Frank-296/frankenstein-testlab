using TestExecutor.Models;

using System.Net.Http.Headers;
using System.Net;

using Newtonsoft.Json;

namespace TestExecutor.Services;

public class ReportsDataStore : IReportsStore<Execution>
{
    const String WebApiURL = "https://localhost:44346";

    private readonly HttpClient client = new()
    {
        BaseAddress = new Uri(WebApiURL)
    };

    private List<Execution> executions;

    public async Task<IList<Execution>> GenerateReportAsync(DateTime startDate, DateTime endDate, String userId)
    {
        if (Preferences.ContainsKey("token"))
        {
            var token = Preferences.Get("token", null) as String;
            var authenticationHeader = new AuthenticationHeaderValue("bearer", token);

            client.DefaultRequestHeaders.Authorization = authenticationHeader;
        }

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var url = $"{WebApiURL}/api/Reports?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}&userId={userId}";
        var result = await client.GetAsync(url);

        if (result.StatusCode == HttpStatusCode.OK)
        {
            var jsonResult = await result.Content.ReadAsStringAsync();

            executions = JsonConvert.DeserializeObject<List<Execution>>(jsonResult);
        }

        return await Task.FromResult(executions);
    }
}