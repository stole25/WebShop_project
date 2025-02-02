using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Webshop_Backend.Data;
using Swashbuckle.AspNetCore;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Webshop_Backend.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => 
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "WebShop API", Version = "v1" });
    
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

builder.Services.AddDbContext<WebShopDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebShopDB"))
);
builder.Services.AddCors(options => 
{
    options.AddPolicy("AllowBlazorFrontend", builder => 
        builder.WithOrigins("https://localhost:7145", "http://localhost:5122")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Autentifikacija
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

// Autorizacija
builder.Services.AddAuthorization();

// BCrypt hasher
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI(options => 
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "WebShop API v1");
    options.RoutePrefix = "swagger"; 
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseAuthorization();
app.UseCors("AllowBlazorFrontend");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();