using Microsoft.AspNetCore.Components;

using TestExecutor.Models;

using System.Net.Http.Headers;
using System.Text;
using System.Net;

using Newtonsoft.Json;

namespace TestExecutor.Services;

public class SuiteExecutionsDataStore : ISuiteExecutionsStore<SuiteExecution>
{
    const String WebApiURL = "https://localhost:44346";

    private readonly HttpClient client = new()
    {
        BaseAddress = new Uri(WebApiURL)
    };

    private List<SuiteExecution> suiteExecutions;

    public async Task<IList<SuiteExecution>> GetSuiteExecutionsAsync(String businessProcessId, String userId)
    {
        if (Preferences.ContainsKey("token"))
        {
            var token = Preferences.Get("token", null) as String;
            var authenticationHeader = new AuthenticationHeaderValue("bearer", token);

            client.DefaultRequestHeaders.Authorization = authenticationHeader;
        }

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var url = $"{WebApiURL}/api/SuiteExecutions?businessProcessId={businessProcessId}&userId={userId}";
        var result = await client.GetAsync(url);

        if (result.StatusCode == HttpStatusCode.OK)
        {
            var jsonResult = await result.Content.ReadAsStringAsync();

            suiteExecutions = JsonConvert.DeserializeObject<List<SuiteExecution>>(jsonResult);
        }

        return await Task.FromResult(suiteExecutions);
    }

    public async Task<SuiteExecution> AddSuiteExecutionAsync(SuiteExecution suiteExecution, String businessProcessId, String testApplicationId, NavigationManager navigationManager)
    {
        if (Preferences.ContainsKey("token"))
        {
            var token = Preferences.Get("token", null) as String;
            var authenticationHeader = new AuthenticationHeaderValue("bearer", token);

            client.DefaultRequestHeaders.Authorization = authenticationHeader;
        }

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var url = $"/api/SuiteExecutions";
        var content = JsonConvert.SerializeObject(suiteExecution);
        var result = await client.PostAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));
        var jsonResult = await result.Content.ReadAsStringAsync();

        suiteExecution = JsonConvert.DeserializeObject<SuiteExecution>(jsonResult);

        switch (result.StatusCode)
        {
            case HttpStatusCode.Created:

                return suiteExecution;

            case HttpStatusCode.Conflict:
                await App.Current.MainPage.DisplayAlert("Warning", "There is already a suite execution registered with these parameters!", "Ok");

                return null;

            case HttpStatusCode.BadRequest:
                await App.Current.MainPage.DisplayAlert("Incorrect", "Could not execute suite!", "Ok");

                return null;

            default:
                await App.Current.MainPage.DisplayAlert("Incorrect", "An unexpected error occurred!", "Ok");
                navigationManager.NavigateTo($"/suiteExecutions/list/{businessProcessId}/{testApplicationId}");

                return null;
        }
    }

    public async Task<SuiteExecution> GetSuiteExecutionAsync(String suiteExecutionId, Boolean loadTestCases)
    {
        if (Preferences.ContainsKey("token"))
        {
            var token = Preferences.Get("token", null) as String;
            var authenticationHeader = new AuthenticationHeaderValue("bearer", token);

            client.DefaultRequestHeaders.Authorization = authenticationHeader;
        }

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        SuiteExecution suiteExecution = null;

        var url = $"{WebApiURL}/api/SuiteExecutions/{suiteExecutionId}/{loadTestCases}";
        var result = await client.GetAsync(url);

        if (result.StatusCode == HttpStatusCode.OK)
        {
            var jsonResult = await result.Content.ReadAsStringAsync();

            suiteExecution = JsonConvert.DeserializeObject<SuiteExecution>(jsonResult);
        }

        return await Task.FromResult(suiteExecution);
	}

	public async Task<SuiteExecution> UpdateSuiteExecutionAsync(SuiteExecution suiteExecution, String businessProcessId, String testApplicationId, NavigationManager navigationManager)
    {
        if (Preferences.ContainsKey("token"))
        {
            var token = Preferences.Get("token", null) as String;
            var authenticationHeader = new AuthenticationHeaderValue("bearer", token);

            client.DefaultRequestHeaders.Authorization = authenticationHeader;
        }

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var url = $"/api/SuiteExecutions/{suiteExecution.SuiteExecutionId}";
        var content = JsonConvert.SerializeObject(suiteExecution);
        var result = await client.PutAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));
        var jsonResult = await result.Content.ReadAsStringAsync();

        suiteExecution = JsonConvert.DeserializeObject<SuiteExecution>(jsonResult);

        switch (result.StatusCode)
        {
            case HttpStatusCode.OK:
                navigationManager.NavigateTo($"/suiteExecutions/result/{suiteExecution.SuiteExecutionId}/{businessProcessId}/{testApplicationId}");

                return await Task.FromResult(suiteExecution);

            case HttpStatusCode.Conflict:
                await App.Current.MainPage.DisplayAlert("Warning", "There is already a suite execution registered with these parameters!", "Ok");

                return null;

            case HttpStatusCode.NotFound:
                await App.Current.MainPage.DisplayAlert("Incorrect", "No suite execution found!", "Ok");
                navigationManager.NavigateTo($"/suiteExecutions/list/{testApplicationId}");

                return null;

            case HttpStatusCode.BadRequest:
                await App.Current.MainPage.DisplayAlert("Incorrect", "Could not update this suite execution!", "Ok");

                return null;

            default:
                await App.Current.MainPage.DisplayAlert("Incorrect", "An unexpected error occurred!", "Ok");
                navigationManager.NavigateTo($"/suiteExecutions/list/{businessProcessId}/{testApplicationId}");

                return null;
        }
    }

    public async Task<Boolean> DeleteSuiteExecutionAsync(String suiteExecutionId)
    {
        if (Preferences.ContainsKey("token"))
        {
            var token = Preferences.Get("token", null) as String;
            var authenticationHeader = new AuthenticationHeaderValue("bearer", token);

            client.DefaultRequestHeaders.Authorization = authenticationHeader;
        }

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var url = $"/api/SuiteExecutions/{suiteExecutionId}";
        var result = await client.DeleteAsync(url);

        switch (result.StatusCode)
        {
            case HttpStatusCode.OK:
                await App.Current.MainPage.DisplayAlert("Correct", "The suite execution was successfully deleted!", "Ok");

                return result.IsSuccessStatusCode;

            case HttpStatusCode.Conflict:
                await App.Current.MainPage.DisplayAlert("Warning", "It cannot be deleted as there is data associated with this suite execution!", "Ok");

                return false;

            case HttpStatusCode.NotFound:
                await App.Current.MainPage.DisplayAlert("Incorrect", "No suite execution found!", "Ok");

                return false;

            case HttpStatusCode.BadRequest:
                await App.Current.MainPage.DisplayAlert("Incorrect", "Could not delete this suite execution!", "Ok");

                return false;

            default:
                await App.Current.MainPage.DisplayAlert("Incorrect", "An unexpected error occurred!", "Ok");

                return false;
        }
    }
}