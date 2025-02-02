using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Webshop_Frontend.Models;

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
            var request = new HttpRequestMessage(HttpMethod.Post, "api/auth/register")
            {
                Content = JsonContent.Create(new { Email = email, Password = password })
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        
            var response = await _http.SendAsync(request);
            response.EnsureSuccessStatusCode(); // Bacit će iznimku za neuspješne statuse
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Greška: {ex.StatusCode} - {ex.Message}");
            throw new Exception("Registracija nije uspjela");
        }
    }

    public async Task Login(string email, string password)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login", new { Email = email, Password = password });

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            await _localStorage.SetItemAsync("authToken", result.Token);
        
            // Asinkrono ažuriranje headera
            _http.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", result.Token);
        }
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");
        _http.DefaultRequestHeaders.Authorization = null;
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

    private class ErrorResponse
    {
        public string Message { get; set; }
    }
}