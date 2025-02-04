using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Webshop_Frontend.Models;

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

    public event Action OnAuthStateChanged;
    public User CurrentUser { get; private set; }
    private void NotifyAuthStateChanged() => OnAuthStateChanged?.Invoke();

    public async Task Register(string email, string password)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/auth/register", new
            {
                Email = email,
                Password = password
            });

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception(ParseErrorMessage(errorContent));
            }

            _navigation.NavigateTo("/login");
        }
        catch (Exception ex)
        {
            throw new Exception($"Registration failed: {ex.Message}");
        }
    }

    public async Task Login(string email, string password)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", new
            {
                Email = email,
                Password = password
            });

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception(ParseLoginError(errorContent));
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            await _localStorage.SetItemAsync("authToken", result.Token);
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Token);

            NotifyAuthStateChanged();
        }
        catch (Exception ex)
        {
            throw new Exception("Neuspješna prijava: " + ex.Message);
        }
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");
        _http.DefaultRequestHeaders.Authorization = null;
        NotifyAuthStateChanged();
        _navigation.NavigateTo("/login", forceLoad: true);
    }

    public async Task<bool> IsLoggedIn()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");
        return !string.IsNullOrWhiteSpace(token);
    }

    public async Task Initialize()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");
        if (!string.IsNullOrWhiteSpace(token))
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    private string ParseLoginError(string json)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("message").GetString() ?? "Neispravni podaci za prijavu";
        }
        catch
        {
            return "Neispravni podaci za prijavu";
        }
    }

    private string ParseErrorMessage(string jsonResponse)
    {
        try
        {
            var json = JsonDocument.Parse(jsonResponse);
            return json.RootElement.GetProperty("message").GetString();
        }
        catch
        {
            return "Unknown error occurred";
        }
    }

    private class LoginResponse
    {
        public string Token { get; set; }
    }
}