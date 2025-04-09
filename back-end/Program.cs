using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Data;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Add a security definition for Bearer Authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter the JWT token with Bearer prefix.",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    // Apply the security scheme globally
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Configure Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT Authentication Setup
var jwtSecret = builder.Configuration["Jwt:Secret"] ?? throw new Exception("JWT Secret is missing!");
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key
        };
    });

// Enable Authorization
builder.Services.AddAuthorization();

// Add Controllers
builder.Services.AddControllers();

// Enable CORS (Cross-Origin Resource Sharing)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("https://komponionas.netlify.app/") // Frontend URL
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Use Serilog for logging
builder.Host.UseSerilog();

// Build the Application
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Swagger UI setup
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();

    // Enable Serilog for logging in Development
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Warning()
        .WriteTo.File("Logs/api-log.txt")
        .Enrich.FromLogContext()
        .CreateLogger();
}

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    // Use custom middleware for logging in Development
    app.UseMiddleware<APILoggingMiddleware>();
}

// Enable CORS Middleware
app.UseCors("AllowFrontend");

// Authentication and Authorization Middleware
app.UseAuthentication();
app.UseAuthorization();

// Map Controllers (API Endpoints)
app.MapControllers();

// Run the app
app.Run();