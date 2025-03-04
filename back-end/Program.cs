using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Data;

var builder = WebApplication.CreateBuilder(args);

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
            .WithOrigins("http://localhost:5173") // Frontend URL
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Build the Application
var app = builder.Build();

// Swagger UI only in Development Environment
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable CORS Middleware
app.UseCors("AllowFrontend");

// Authentication and Authorization Middleware
app.UseAuthentication();
app.UseAuthorization();

// Map Controllers (API Endpoints)
app.MapControllers();

// Run the app
app.Run();