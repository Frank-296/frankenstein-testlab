using Microsoft.AspNetCore.Components;

using TestExecutor.Models;

using System.Net.Http.Headers;
using System.Text;
using System.Net;

using Newtonsoft.Json;

namespace TestExecutor.Services;

public class TestCasesDataStore : ITestCasesStore<TestCase>
{
    const String WebApiURL = "https://localhost:44346";

    private readonly HttpClient client = new()
    {
        BaseAddress = new Uri(WebApiURL)
    };

    private List<TestCase> testCases;

    public async Task<IList<TestCase>> GetTestCasesAsync(String businessProcessId)
    {
        if (Preferences.ContainsKey("token"))
        {
            var token = Preferences.Get("token", null) as String;
            var authenticationHeader = new AuthenticationHeaderValue("bearer", token);

            client.DefaultRequestHeaders.Authorization = authenticationHeader;
        }

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var url = $"{WebApiURL}/api/TestCases?id={businessProcessId}";
        var result = await client.GetAsync(url);

        if (result.StatusCode == HttpStatusCode.OK)
        {
            var jsonResult = await result.Content.ReadAsStringAsync();

            testCases = JsonConvert.DeserializeObject<List<TestCase>>(jsonResult);
        }

        return await Task.FromResult(testCases);
    }

    public async Task<TestCase> AddTestCaseAsync(TestCase testCase, String testApplicationId, NavigationManager navigationManager)
    {
        if (Preferences.ContainsKey("token"))
        {
            var token = Preferences.Get("token", null) as String;
            var authenticationHeader = new AuthenticationHeaderValue("bearer", token);

            client.DefaultRequestHeaders.Authorization = authenticationHeader;
        }

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var url = $"/api/TestCases";
        var content = JsonConvert.SerializeObject(testCase);
        var result = await client.PostAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));
        var jsonResult = await result.Content.ReadAsStringAsync();

        testCase = JsonConvert.DeserializeObject<TestCase>(jsonResult);

        switch (result.StatusCode)
        {
            case HttpStatusCode.Created:
                await App.Current.MainPage.DisplayAlert("Correct", "The test case has been successfully added!", "Ok");
                navigationManager.NavigateTo($"/testCases/list/{testCase.BusinessProcessId}/{testApplicationId}");

                return testCase;

            case HttpStatusCode.Conflict:
                await App.Current.MainPage.DisplayAlert("Warning", "There is already a test case registered with this name!", "Ok");

                return null;

            case HttpStatusCode.BadRequest:
                await App.Current.MainPage.DisplayAlert("Incorrect", "This test case could not be added!", "Ok");

                return null;

            default:
                await App.Current.MainPage.DisplayAlert("Incorrect", "An unexpected error occurred!", "Ok");
                navigationManager.NavigateTo($"/testCases/list/{testCase.BusinessProcessId}/{testApplicationId}");

                return null;
        }
    }

    public async Task<TestCase> GetTestCaseAsync(String testCaseId)
    {
        if (Preferences.ContainsKey("token"))
        {
            var token = Preferences.Get("token", null) as String;
            var authenticationHeader = new AuthenticationHeaderValue("bearer", token);

            client.DefaultRequestHeaders.Authorization = authenticationHeader;
        }

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        TestCase testCase = null;

        var url = $"{WebApiURL}/api/TestCases/{testCaseId}";
        var result = await client.GetAsync(url);

        if (result.StatusCode == HttpStatusCode.OK)
        {
            var jsonResult = await result.Content.ReadAsStringAsync();

            testCase = JsonConvert.DeserializeObject<TestCase>(jsonResult);
        }

        return await Task.FromResult(testCase);
    }

    public async Task<TestCase> UpdateTestCaseAsync(TestCase testCase, String testApplicationId, NavigationManager navigationManager)
    {
        if (Preferences.ContainsKey("token"))
        {
            var token = Preferences.Get("token", null) as String;
            var authenticationHeader = new AuthenticationHeaderValue("bearer", token);

            client.DefaultRequestHeaders.Authorization = authenticationHeader;
        }

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var url = $"/api/TestCases/{testCase.TestCaseId}";
        var content = JsonConvert.SerializeObject(testCase);
        var result = await client.PutAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));
        var jsonResult = await result.Content.ReadAsStringAsync();

        testCase = JsonConvert.DeserializeObject<TestCase>(jsonResult);

        switch (result.StatusCode)
        {
            case HttpStatusCode.OK:
                await App.Current.MainPage.DisplayAlert("Correct", "The test case has been successfully updated!", "Ok");
                navigationManager.NavigateTo($"/testCases/list/{testCase.BusinessProcessId}/{testApplicationId}");

                return await Task.FromResult(testCase);

            case HttpStatusCode.Conflict:
                await App.Current.MainPage.DisplayAlert("Warning", "There is already a test case registered with this name!", "Ok");

                return null;

            case HttpStatusCode.NotFound:
                await App.Current.MainPage.DisplayAlert("Incorrect", "Test case not found!", "Ok");
                navigationManager.NavigateTo($"/testCases/list/{testCase.BusinessProcessId}/{testApplicationId}");

                return null;

            case HttpStatusCode.BadRequest:
                await App.Current.MainPage.DisplayAlert("Incorrect", "Could not update this test case!", "Ok");

                return null;

            default:
                await App.Current.MainPage.DisplayAlert("Incorrect", "An unexpected error occurred!", "Ok");
                navigationManager.NavigateTo($"/testCases/list/{testCase.BusinessProcessId}/{testApplicationId}");

                return null;
        }
    }

    public async Task<Boolean> DeleteTestCaseAsync(String testCaseId)
    {
        if (Preferences.ContainsKey("token"))
        {
            var token = Preferences.Get("token", null) as String;
            var authenticationHeader = new AuthenticationHeaderValue("bearer", token);

            client.DefaultRequestHeaders.Authorization = authenticationHeader;
        }

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var url = $"/api/TestCases/{testCaseId}";
        var result = await client.DeleteAsync(url);

        switch (result.StatusCode)
        {
            case HttpStatusCode.OK:
                await App.Current.MainPage.DisplayAlert("Correct", "The test case was successfully deleted!", "Ok");

                return result.IsSuccessStatusCode;

            case HttpStatusCode.Conflict:
                await App.Current.MainPage.DisplayAlert("Warning", "It cannot be deleted as there is data associated with this test case!", "Ok");

                return false;

            case HttpStatusCode.NotFound:
                await App.Current.MainPage.DisplayAlert("Incorrect", "Test case not found!", "Ok");

                return false;

            case HttpStatusCode.BadRequest:
                await App.Current.MainPage.DisplayAlert("Incorrect", "Could not delete this test case!", "Ok");

                return false;

            default:
                await App.Current.MainPage.DisplayAlert("Incorrect", "An unexpected error occurred!", "Ok");

                return false;
        }
    }
}