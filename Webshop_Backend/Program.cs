using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Webshop_Backend.Data;
using Webshop_Backend.Services;

var builder = WebApplication.CreateBuilder(args);

// CORS konfiguracija
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.WithOrigins("https://localhost:7145") // Postavi ispravan URL frontend aplikacije
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials());
});

// Dodavanje kontrolera
builder.Services.AddControllers();

// Swagger konfiguracija
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebShop API", Version = "v1" });
});

// Database konfiguracija
builder.Services.AddDbContext<WebShopDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebShopDB")));

// Autentifikacija i autorizacija
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();

// Lokalizacija
var cultureInfo = new CultureInfo("hr-HR"); // Kultura "hr-HR" (ili promijeni prema potrebi)
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Dodavanje vlastitih servisa
/// builder.Services.AddScoped<CheckoutService>(); // Dodaj ako koristiš CheckoutService

var app = builder.Build();

// Middleware za Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebShop API v1");
    });
}

// Middleware redoslijed
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

// Lokalizacija middleware
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(cultureInfo)
});

// Mapiranje ruta za kontrolere
app.MapControllers();

app.Run();