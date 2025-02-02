using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;
    private readonly NavigationManager _navigation;

    public AuthService(HttpClient http, ILocalStorageService localStorage, NavigationManager navigation)
    {
        _http = http;
        _localStorage = localStorage;
        _navigation = navigation;
    }

    public async Task Register(string email, string password)
    {
        var response = await _http.PostAsJsonAsync("api/auth/register", new { Email = email, Password = password });

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Registration failed: {errorMessage}"); // Prikaz greške u konzoli
            throw new Exception($"Registration failed: {errorMessage}");
        }

        _navigation.NavigateTo("/login");
    }

    public async Task Login(string email, string password)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login", new { Email = email, Password = password });

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Login failed: {errorMessage}"); // Prikaz greške u konzoli
            throw new Exception($"Login failed: {errorMessage}");
        }

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        await _localStorage.SetItemAsync("authToken", result.Token);
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", result.Token);

        _navigation.NavigateTo("/");
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");
        _http.DefaultRequestHeaders.Authorization = null;

        // Preusmjerite korisnika na login stranicu nakon odjave
        _navigation.NavigateTo("/login");
    }

    public async Task<bool> IsLoggedIn()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");
        return !string.IsNullOrWhiteSpace(token);
    }

    private class LoginResponse
    {
        public string Token { get; set; }
    }
}
