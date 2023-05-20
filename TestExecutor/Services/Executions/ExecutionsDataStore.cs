using Microsoft.AspNetCore.Components;

using TestExecutor.Models;
using TestLab.Utilities;

using System.Net.Http.Headers;
using System.Text;
using System.Net;

using Newtonsoft.Json;

namespace TestExecutor.Services;

public class ExecutionsDataStore : IExecutionsStore<Execution>
{
    const String WebApiURL = "https://localhost:44346";

    private readonly HttpClient client = new()
    {
        BaseAddress = new Uri(WebApiURL)
    };

    private List<Execution> executions;

    public async Task<IList<Execution>> GetExecutionsAsync(String testCaseId, String userId, ExecutionStatus? executionStatus)
    {
        if (Preferences.ContainsKey("token"))
        {
            var token = Preferences.Get("token", null) as String;
            var authenticationHeader = new AuthenticationHeaderValue("bearer", token);

            client.DefaultRequestHeaders.Authorization = authenticationHeader;
        }

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var url = $"{WebApiURL}/api/Executions?testCaseId={testCaseId}&userId={userId}&executionStatus={executionStatus}";
        var result = await client.GetAsync(url);

        if (result.StatusCode == HttpStatusCode.OK)
        {
            var jsonResult = await result.Content.ReadAsStringAsync();

            executions = JsonConvert.DeserializeObject<List<Execution>>(jsonResult);
        }

        return await Task.FromResult(executions);
    }

    public async Task<Execution> AddExecutionAsync(Execution execution, String businessProcessId, String testApplicationId, NavigationManager navigationManager, String from, Boolean showResult)
    {
        if (Preferences.ContainsKey("token"))
        {
            var token = Preferences.Get("token", null) as String;
            var authenticationHeader = new AuthenticationHeaderValue("bearer", token);

            client.DefaultRequestHeaders.Authorization = authenticationHeader;
        }

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var url = $"/api/Executions";
        var content = JsonConvert.SerializeObject(execution);
        var result = await client.PostAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));
        var jsonResult = await result.Content.ReadAsStringAsync();

        execution = JsonConvert.DeserializeObject<Execution>(jsonResult);

        switch (result.StatusCode)
        {
            case HttpStatusCode.Created:
                if (showResult)
                    navigationManager.NavigateTo($"/executions/result/{from}/{execution.ExecutionId}/{execution.TestCaseId}/{businessProcessId}/{testApplicationId}");

                return execution;

            case HttpStatusCode.Conflict:
                await App.Current.MainPage.DisplayAlert("Warning", "There is already a execution registered with these parameters!", "Ok");

                return null;

            case HttpStatusCode.BadRequest:
                await App.Current.MainPage.DisplayAlert("Incorrect", "Could not execute!", "Ok");

                return null;

            default:
                await App.Current.MainPage.DisplayAlert("Incorrect", "An unexpected error occurred!", "Ok");
                navigationManager.NavigateTo($"/executions/list/{execution.TestCaseId}/{businessProcessId}/{testApplicationId}");

                return null;
        }
    }

    public async Task<Execution> GetExecutionAsync(String executionId, Boolean loadTestReport, Boolean loadTestData)
    {
        if (Preferences.ContainsKey("token"))
        {
            var token = Preferences.Get("token", null) as String;
            var authenticationHeader = new AuthenticationHeaderValue("bearer", token);

            client.DefaultRequestHeaders.Authorization = authenticationHeader;
        }

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        Execution execution = null;

        var url = $"{WebApiURL}/api/Executions/{executionId}/{loadTestReport}/{loadTestData}";
        var result = await client.GetAsync(url);

        if (result.StatusCode == HttpStatusCode.OK)
        {
            var jsonResult = await result.Content.ReadAsStringAsync();

            execution = JsonConvert.DeserializeObject<Execution>(jsonResult);
        }

        return await Task.FromResult(execution);
    }

    public async Task<Execution> UpdateExecutionAsync(Execution execution, String businessProcessId, String testApplicationId, NavigationManager navigationManager, String from, Boolean showResult)
    {
        if (Preferences.ContainsKey("token"))
        {
            var token = Preferences.Get("token", null) as String;
            var authenticationHeader = new AuthenticationHeaderValue("bearer", token);

            client.DefaultRequestHeaders.Authorization = authenticationHeader;
        }

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var url = $"/api/Executions/{execution.ExecutionId}";
        var content = JsonConvert.SerializeObject(execution);
        var result = await client.PutAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));
        var jsonResult = await result.Content.ReadAsStringAsync();

        execution = JsonConvert.DeserializeObject<Execution>(jsonResult);

        switch (result.StatusCode)
        {
            case HttpStatusCode.OK:
                if (showResult)
                    navigationManager.NavigateTo($"/executions/result/{from}/{execution.ExecutionId}/{execution.TestCaseId}/{businessProcessId}/{testApplicationId}");

                return await Task.FromResult(execution);

            case HttpStatusCode.Conflict:
                await App.Current.MainPage.DisplayAlert("Warning", "There is already a execution registered with these parameters!", "Ok");

                return null;

            case HttpStatusCode.NotFound:
                await App.Current.MainPage.DisplayAlert("Incorrect", "No execution found!", "Ok");
                navigationManager.NavigateTo($"/executions/list/{execution.TestCaseId}/{testApplicationId}");

                return null;

            case HttpStatusCode.BadRequest:
                await App.Current.MainPage.DisplayAlert("Incorrect", "Could not update this execution!", "Ok");

                return null;

            default:
                await App.Current.MainPage.DisplayAlert("Incorrect", "An unexpected error occurred!", "Ok");
                navigationManager.NavigateTo($"/executions/list/{execution.TestCaseId}/{businessProcessId}/{testApplicationId}");

                return null;
        }
    }

    public async Task<Boolean> DeleteExecutionAsync(String executionId)
    {
        if (Preferences.ContainsKey("token"))
        {
            var token = Preferences.Get("token", null) as String;
            var authenticationHeader = new AuthenticationHeaderValue("bearer", token);

            client.DefaultRequestHeaders.Authorization = authenticationHeader;
        }

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var url = $"/api/Executions/{executionId}";
        var result = await client.DeleteAsync(url);

        switch (result.StatusCode)
        {
            case HttpStatusCode.OK:
                await App.Current.MainPage.DisplayAlert("Correct", "The execution was successfully deleted!", "Ok");

                return result.IsSuccessStatusCode;

            case HttpStatusCode.Conflict:
                await App.Current.MainPage.DisplayAlert("Warning", "It cannot be deleted as there is data associated with this execution!", "Ok");

                return false;

            case HttpStatusCode.NotFound:
                await App.Current.MainPage.DisplayAlert("Incorrect", "No execution found!", "Ok");

                return false;

            case HttpStatusCode.BadRequest:
                await App.Current.MainPage.DisplayAlert("Incorrect", "Could not delete this execution!", "Ok");

                return false;

            default:
                await App.Current.MainPage.DisplayAlert("Incorrect", "An unexpected error occurred!", "Ok");

                return false;
        }
    }
}