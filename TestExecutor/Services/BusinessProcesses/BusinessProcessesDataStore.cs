using TestExecutor.Models;

using System.Net.Http.Headers;
using System.Text;
using System.Net;

using Newtonsoft.Json;

namespace TestExecutor.Services;

public class BusinessProcessesDataStore : IBusinessProcessesStore<BusinessProcess>
{
    const String WebApiURL = "https://localhost:44346";

    private readonly HttpClient client = new()
    {
        BaseAddress = new Uri(WebApiURL)
    };

    private List<BusinessProcess> businessProcesses;

    public async Task<IList<BusinessProcess>> GetBusinessProcessesAsync(String testApplicationId)
    {
        if (Preferences.ContainsKey("token"))
        {
            var token = Preferences.Get("token", null) as String;
            var authenticationHeader = new AuthenticationHeaderValue("bearer", token);

            client.DefaultRequestHeaders.Authorization = authenticationHeader;
        }

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var url = $"{WebApiURL}/api/BusinessProcesses?id={testApplicationId}";
        var result = await client.GetAsync(url);

        if (result.StatusCode == HttpStatusCode.OK)
        {
            var jsonResult = await result.Content.ReadAsStringAsync();

            businessProcesses = JsonConvert.DeserializeObject<List<BusinessProcess>>(jsonResult);
        }

        return await Task.FromResult(businessProcesses);
    }

    public async Task<BusinessProcess> AddBusinessProcessAsync(BusinessProcess businessProcess)
    {
        if (Preferences.ContainsKey("token"))
        {
            var token = Preferences.Get("token", null) as String;
            var authenticationHeader = new AuthenticationHeaderValue("bearer", token);

            client.DefaultRequestHeaders.Authorization = authenticationHeader;
        }

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var url = $"/api/BusinessProcesses";
        var content = JsonConvert.SerializeObject(businessProcess);
        var result = await client.PostAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));
        var jsonResult = await result.Content.ReadAsStringAsync();

        businessProcess = JsonConvert.DeserializeObject<BusinessProcess>(jsonResult);

        switch (result.StatusCode)
        {
            case HttpStatusCode.Created:
                await App.Current.MainPage.DisplayAlert("Correct", "The business process has been successfully added!", "Ok");

                return businessProcess;

            case HttpStatusCode.Conflict:
                await App.Current.MainPage.DisplayAlert("Warning", "There is already a business process registered with this name!", "Ok");

                return null;

            case HttpStatusCode.BadRequest:
                await App.Current.MainPage.DisplayAlert("Incorrect", "This business process could not be added!", "Ok");

                return null;

            default:
                await App.Current.MainPage.DisplayAlert("Incorrect", "An unexpected error occurred!", "Ok");

                return null;
        }
    }

    public async Task<BusinessProcess> GetBusinessProcessAsync(String businessProcessId)
    {
        if (Preferences.ContainsKey("token"))
        {
            var token = Preferences.Get("token", null) as String;
            var authenticationHeader = new AuthenticationHeaderValue("bearer", token);

            client.DefaultRequestHeaders.Authorization = authenticationHeader;
        }

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        BusinessProcess businessProcess = null;

        var url = $"{WebApiURL}/api/BusinessProcesses/{businessProcessId}";
        var result = await client.GetAsync(url);

        if (result.StatusCode == HttpStatusCode.OK)
        {
            var jsonResult = await result.Content.ReadAsStringAsync();

            businessProcess = JsonConvert.DeserializeObject<BusinessProcess>(jsonResult);
        }

        return await Task.FromResult(businessProcess);
    }

    public async Task<BusinessProcess> UpdateBusinessProcessAsync(BusinessProcess businessProcess)
    {
        if (Preferences.ContainsKey("token"))
        {
            var token = Preferences.Get("token", null) as String;
            var authenticationHeader = new AuthenticationHeaderValue("bearer", token);

            client.DefaultRequestHeaders.Authorization = authenticationHeader;
        }

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var url = $"/api/BusinessProcesses/{businessProcess.BusinessProcessId}";
        var content = JsonConvert.SerializeObject(businessProcess);
        var result = await client.PutAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));
        var jsonResult = await result.Content.ReadAsStringAsync();

        businessProcess = JsonConvert.DeserializeObject<BusinessProcess>(jsonResult);

        switch (result.StatusCode)
        {
            case HttpStatusCode.OK:
                await App.Current.MainPage.DisplayAlert("Correct", "The business process has been successfully updated!", "Ok");

                return await Task.FromResult(businessProcess);

            case HttpStatusCode.Conflict:
                await App.Current.MainPage.DisplayAlert("Warning", "There is already a business process registered with this name!", "Ok");

                return null;

            case HttpStatusCode.NotFound:
                await App.Current.MainPage.DisplayAlert("Incorrect", "Business process not found!", "Ok");

                return null;

            case HttpStatusCode.BadRequest:
                await App.Current.MainPage.DisplayAlert("Incorrect", "Could not update this business process!", "Ok");

                return null;

            default:
                await App.Current.MainPage.DisplayAlert("Incorrect", "An unexpected error occurred!", "Ok");

                return null;
        }
    }

    public async Task<Boolean> DeleteBusinessProcessAsync(String businessProcessId)
    {
        if (Preferences.ContainsKey("token"))
        {
            var token = Preferences.Get("token", null) as String;
            var authenticationHeader = new AuthenticationHeaderValue("bearer", token);

            client.DefaultRequestHeaders.Authorization = authenticationHeader;
        }

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var url = $"/api/BusinessProcesses/{businessProcessId}";
        var result = await client.DeleteAsync(url);

        switch (result.StatusCode)
        {
            case HttpStatusCode.OK:
                await App.Current.MainPage.DisplayAlert("Correct", "The business process was successfully deleted!", "Ok");

                return result.IsSuccessStatusCode;

            case HttpStatusCode.Conflict:
                await App.Current.MainPage.DisplayAlert("Warning", "It cannot be deleted as there is data associated with this business process!", "Ok");

                return false;

            case HttpStatusCode.NotFound:
                await App.Current.MainPage.DisplayAlert("Incorrect", "Business process not found!", "Ok");

                return false;

            case HttpStatusCode.BadRequest:
                await App.Current.MainPage.DisplayAlert("Incorrect", "Could not delete this business process!", "Ok");

                return false;

            default:
                await App.Current.MainPage.DisplayAlert("Incorrect", "An unexpected error occurred!", "Ok");

                return false;
        }
    }
}