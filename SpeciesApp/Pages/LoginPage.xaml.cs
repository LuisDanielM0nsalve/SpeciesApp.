using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace SimpleLoginApp.Pages
{
    public partial class LoginPage : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            var username = usernameEntry.Text;
            var password = passwordEntry.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Username and password are required.", "OK");
                return;
            }

            var loginRequest = new { Username = username, Password = password };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:44395/api/Authentications/login", loginRequest);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    var token = responseData?.Token;

                    tokenLabel.Text = $"Token: {token}";
                }
                else
                {
                    await DisplayAlert("Error", "Invalid username or password", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
    }
}
