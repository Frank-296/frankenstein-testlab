using TestExecutor.Models;

using System.Net.Http.Headers;
using System.Text;
using System.Net;

using Newtonsoft.Json;

namespace TestExecutor.Services;

public class AccountsDataStore : IAccountsStore<LoginViewModel, GetProfileViewModel, UpdateProfileViewModel, ChangePasswordViewModel>
{
    const String WebApiURL = "https://localhost:44346";

    private readonly HttpClient client = new()
    {
        BaseAddress = new Uri(WebApiURL)
    };

    public async Task<String> LoginAsync(LoginViewModel login)
    {
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var url = $"{WebApiURL}/api/Account/Login";
        var content = JsonConvert.SerializeObject(login);
        var result = await client.PostAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));
        var jsonResult = await result.Content.ReadAsStringAsync();

        GetUserTokenViewModel getToken = JsonConvert.DeserializeObject<GetUserTokenViewModel>(jsonResult);

        switch (result.StatusCode)
        {
            case HttpStatusCode.OK:
                Preferences.Set("id", getToken.Id);
                Preferences.Set("companyId", getToken.CompanyId);
                Preferences.Set("role", getToken.Role);
                Preferences.Set("token", getToken.Token);
                Preferences.Set("expiration", getToken.Expiration);

                await SecureStorage.SetAsync("isLogged", "1");

                return await Task.FromResult("1");

            case HttpStatusCode.Unauthorized:
                await App.Current.MainPage.DisplayAlert("Incorrect", "Invalid login!", "Ok");

                return null;

            default:
                await App.Current.MainPage.DisplayAlert("Incorrect", "An unexpected error occurred!", "Ok");

                return null;
        }
    }

    public async Task<GetProfileViewModel> GetProfileAsync(String userId)
    {
        if (Preferences.ContainsKey("token"))
        {
            var token = Preferences.Get("token", null) as String;
            var authenticationHeader = new AuthenticationHeaderValue("bearer", token);

            client.DefaultRequestHeaders.Authorization = authenticationHeader;
        }

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        GetProfileViewModel profile = null;

        var url = $"{WebApiURL}/api/Account/{userId}";
        var result = await client.GetAsync(url);

        if (result.StatusCode == HttpStatusCode.OK)
        {
            var jsonResult = await result.Content.ReadAsStringAsync();

            profile = JsonConvert.DeserializeObject<GetProfileViewModel>(jsonResult);

            Preferences.Set("tester", profile.DisplayName);
            Preferences.Set("company", profile.Company);
            Preferences.Set("email", profile.Email);
            Preferences.Set("phoneNumber", profile.PhoneNumber);
            Preferences.Set("registrationDate", profile.RegistrationDate);
            Preferences.Set("setPasswordDate", profile.SetPasswordDate);
        }

        return await Task.FromResult(profile);
    }

    public async Task<UpdateProfileViewModel> UpdateProfileAsync(UpdateProfileViewModel profile)
    {
        if (Preferences.ContainsKey("token"))
        {
            var token = Preferences.Get("token", null) as String;
            var authenticationHeader = new AuthenticationHeaderValue("bearer", token);

            client.DefaultRequestHeaders.Authorization = authenticationHeader;
        }

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        Preferences.Set("phoneNumber", profile.PhoneNumber);
        Preferences.Set("tester", $"{profile.Name} {profile.LastName} {profile.MothersLastName}");

        var url = $"{WebApiURL}/api/Account/{profile.Id}";
        var content = JsonConvert.SerializeObject(profile);
        var result = await client.PutAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));
        var jsonResult = await result.Content.ReadAsStringAsync();

        profile = JsonConvert.DeserializeObject<UpdateProfileViewModel>(jsonResult);

        switch (result.StatusCode)
        {
            case HttpStatusCode.OK:
                await App.Current.MainPage.DisplayAlert("Correct", "Your profile was successfully updated!", "Ok");

                return await Task.FromResult(profile);

            case HttpStatusCode.NotFound:
                await App.Current.MainPage.DisplayAlert("Incorrect", "Your profile was not found!", "Ok");

                return null;

            case HttpStatusCode.BadRequest:
                await App.Current.MainPage.DisplayAlert("Incorrect", "Could not update your profile!", "Ok");

                return null;

            default:
                await App.Current.MainPage.DisplayAlert("Incorrect", "An unexpected error occurred!", "Ok");

                return null;
        }
    }

    public async Task<Boolean> ChangePasswordAsync(ChangePasswordViewModel changePassword)
    {
        if (Preferences.ContainsKey("token"))
        {
            var token = Preferences.Get("token", null) as String;
            var authenticationHeader = new AuthenticationHeaderValue("bearer", token);

            client.DefaultRequestHeaders.Authorization = authenticationHeader;
        }

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


        var url = $"{WebApiURL}/api/Account/ChangePassword";
        var content = JsonConvert.SerializeObject(changePassword);
        var result = await client.PostAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));

        switch (result.StatusCode)
        {
            case HttpStatusCode.OK:
                await App.Current.MainPage.DisplayAlert("Correct", "Your password was successfully updated!", "Ok");

                return result.IsSuccessStatusCode;

            case HttpStatusCode.Conflict:
                await App.Current.MainPage.DisplayAlert("Warning", "Your current password is wrong!", "Ok");

                return false;

            case HttpStatusCode.NotFound:
                await App.Current.MainPage.DisplayAlert("Incorrect", "Your username was not found!", "Ok");

                return false;

            case HttpStatusCode.BadRequest:
                await App.Current.MainPage.DisplayAlert("Incorrect", "Could not update your password!", "Ok");

                return false;

            default:
                await App.Current.MainPage.DisplayAlert("Incorrect", "An unexpected error occurred!", "Ok");

                return false;
        }
    }
}