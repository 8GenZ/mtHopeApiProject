using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using mtHopeApiProject.Data;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add User Secrets in Development environment
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

// Disable AzureBlobTraceListener completely by clearing all providers
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // Add console logging for local debugging

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder =>
        {
            builder.WithOrigins("https://localhost:7181") // Add the URL of your UI project
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});

// Add services to the container
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"))
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddInMemoryTokenCaches();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScopePolicy", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scp", "api.write"); // Ensure the access token has this scope
    });
});

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null)
    )
    .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information) // Log queries
    .EnableSensitiveDataLogging() // Log sensitive data like parameter values
);

// Add Logging Services
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole(); // Log to the console for diagnostics
});

// Add Swagger/OpenAPI support using Swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Recipient API",
        Version = "v1",
        Description = "API for managing recipients of forms."
    });
});

var app = builder.Build();

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Unhandled exception: {ex}");
        throw;
    }
});

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Recipient API v1");
    options.RoutePrefix = string.Empty; // Serve Swagger at the root
});

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
app.UseCors("AllowSpecificOrigins");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
