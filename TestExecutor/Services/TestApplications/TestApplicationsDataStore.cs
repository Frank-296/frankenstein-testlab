using TestExecutor.Models;

using System.Net.Http.Headers;
using System.Text;
using System.Net;

using Newtonsoft.Json;

namespace TestExecutor.Services;

public class TestApplicationsDataStore : ITestApplicationsStore<TestApplication>
{
    const String WebApiURL = "https://localhost:44346";

    private readonly HttpClient client = new()
    {
        BaseAddress = new Uri(WebApiURL)
    };

    private List<TestApplication> testApplications;

    public async Task<IList<TestApplication>> GetTestApplicationsAsync()
    {
        if (Preferences.ContainsKey("token"))
        {
            var token = Preferences.Get("token", null) as String;
            var authenticationHeader = new AuthenticationHeaderValue("bearer", token);

            client.DefaultRequestHeaders.Authorization = authenticationHeader;
        }

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var url = $"{WebApiURL}/api/TestApplications";
        var result = await client.GetAsync(url);

        if (result.StatusCode == HttpStatusCode.OK)
        {
            var jsonResult = await result.Content.ReadAsStringAsync();

            testApplications = JsonConvert.DeserializeObject<List<TestApplication>>(jsonResult);
        }

        return await Task.FromResult(testApplications);
    }

    public async Task<TestApplication> AddTestApplicationAsync(TestApplication testApplication)
    {
        if (Preferences.ContainsKey("token"))
        {
            var token = Preferences.Get("token", null) as String;
            var authenticationHeader = new AuthenticationHeaderValue("bearer", token);

            client.DefaultRequestHeaders.Authorization = authenticationHeader;
        }

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var url = $"/api/TestApplications";
        var content = JsonConvert.SerializeObject(testApplication);
        var result = await client.PostAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));
        var jsonResult = await result.Content.ReadAsStringAsync();

        testApplication = JsonConvert.DeserializeObject<TestApplication>(jsonResult);

        switch (result.StatusCode)
        {
            case HttpStatusCode.Created:
                await App.Current.MainPage.DisplayAlert("Correct", "The application has been successfully added!", "Ok");

                return testApplication;

            case HttpStatusCode.Conflict:
                await App.Current.MainPage.DisplayAlert("Warning", "There is already an application registered with this name!", "Ok");

                return null;

            case HttpStatusCode.BadRequest:
                await App.Current.MainPage.DisplayAlert("Incorrect", "This application could not be added!", "Ok");

                return null;

            default:
                await App.Current.MainPage.DisplayAlert("Incorrect", "An unexpected error occurred!", "Ok");

                return null;
        }
    }

    public async Task<TestApplication> GetTestApplicationAsync(String testApplicationId)
    {
        if (Preferences.ContainsKey("token"))
        {
            var token = Preferences.Get("token", null) as String;
            var authenticationHeader = new AuthenticationHeaderValue("bearer", token);

            client.DefaultRequestHeaders.Authorization = authenticationHeader;
        }

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        TestApplication testApplication = null;

        var url = $"{WebApiURL}/api/TestApplications/{testApplicationId}";
        var result = await client.GetAsync(url);

        if (result.StatusCode == HttpStatusCode.OK)
        {
            var jsonResult = await result.Content.ReadAsStringAsync();

            testApplication = JsonConvert.DeserializeObject<TestApplication>(jsonResult);
        }

        return await Task.FromResult(testApplication);
    }

    public async Task<TestApplication> UpdateTestApplicationAsync(TestApplication testApplication)
    {
        if (Preferences.ContainsKey("token"))
        {
            var token = Preferences.Get("token", null) as String;
            var authenticationHeader = new AuthenticationHeaderValue("bearer", token);

            client.DefaultRequestHeaders.Authorization = authenticationHeader;
        }

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var url = $"/api/TestApplications/{testApplication.TestApplicationId}";
        var content = JsonConvert.SerializeObject(testApplication);
        var result = await client.PutAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));
        var jsonResult = await result.Content.ReadAsStringAsync();

        testApplication = JsonConvert.DeserializeObject<TestApplication>(jsonResult);

        switch (result.StatusCode)
        {
            case HttpStatusCode.OK:
                await App.Current.MainPage.DisplayAlert("Correct", "The application has been successfully updated!", "Ok");

                return await Task.FromResult(testApplication);

            case HttpStatusCode.Conflict:
                await App.Current.MainPage.DisplayAlert("Warning", "There is already an application registered with this name!", "Ok");

                return null;

            case HttpStatusCode.NotFound:
                await App.Current.MainPage.DisplayAlert("Incorrect", "Application not found!", "Ok");

                return null;

            case HttpStatusCode.BadRequest:
                await App.Current.MainPage.DisplayAlert("Incorrect", "Could not update this application!", "Ok");

                return null;

            default:
                await App.Current.MainPage.DisplayAlert("Incorrect", "An unexpected error occurred!", "Ok");

                return null;
        }
    }

    public async Task<Boolean> DeleteTestApplicationAsync(String testApplicationId)
    {
        if (Preferences.ContainsKey("token"))
        {
            var token = Preferences.Get("token", null) as String;
            var authenticationHeader = new AuthenticationHeaderValue("bearer", token);

            client.DefaultRequestHeaders.Authorization = authenticationHeader;
        }

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var url = $"/api/TestApplications/{testApplicationId}";
        var result = await client.DeleteAsync(url);

        switch (result.StatusCode)
        {
            case HttpStatusCode.OK:
                await App.Current.MainPage.DisplayAlert("Correct", "The application was successfully deleted!", "Ok");

                return result.IsSuccessStatusCode;

            case HttpStatusCode.Conflict:
                await App.Current.MainPage.DisplayAlert("Warning", "It cannot be deleted as there is data associated with this application!", "Ok");

                return false;

            case HttpStatusCode.NotFound:
                await App.Current.MainPage.DisplayAlert("Incorrect", "Application not found!", "Ok");

                return false;

            case HttpStatusCode.BadRequest:
                await App.Current.MainPage.DisplayAlert("Incorrect", "Could not delete this application!", "Ok");

                return false;

            default:
                await App.Current.MainPage.DisplayAlert("Incorrect", "An unexpected error occurred!", "Ok");

                return false;
        }
    }
}