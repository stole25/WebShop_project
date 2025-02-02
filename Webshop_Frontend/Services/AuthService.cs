using System.Net.Http.Json;

namespace Webshop_Frontend.Services;

// Webshop_Frontend/Services/AuthService.cs
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;

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
        var response = await _http.PostAsJsonAsync("api/auth/register", new { Email = email, Password = password });

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new Exception($"Registration failed: {errorMessage}");
        }
    }

    public async Task Login(string email, string password)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login", new { email, password });

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            await _localStorage.SetItemAsync("authToken", result.token);
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", result.token);
            _navigation.NavigateTo("/");
        }
        else
        {
            // Prikaz poruke korisniku (možda kroz dijalog ili obavest)
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Login failed: {error}");
        }
    }


    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");
        _http.DefaultRequestHeaders.Authorization = null;
        _navigation.NavigateTo("/login");
    }

    private class LoginResponse
    {
        public string token { get; set; }
    }
}
