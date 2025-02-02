using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Webshop_Frontend.Models;
using Webshop_Frontend.Services;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;
    private readonly NavigationManager _navigation;

    public AuthService(
        HttpClient http,
        ILocalStorageService localStorage,
        NavigationManager navigation)
    {
        _http = http;
        _localStorage = localStorage;
        _navigation = navigation;
    }
    
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
                throw new Exception(ParseErrorMessage(errorContent));
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            await _localStorage.SetItemAsync("authToken", result.Token);
            
            // Update HTTP headers and force reload
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Token);
            _navigation.NavigateTo("/", forceLoad: true);
        }
        catch (Exception ex)
        {
            throw new Exception($"Login failed: {ex.Message}");
        }
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");
        _http.DefaultRequestHeaders.Authorization = null;
        _navigation.NavigateTo("/login", forceLoad: true);
    }

    public async Task<bool> IsLoggedIn()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");
        return !string.IsNullOrWhiteSpace(token);
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