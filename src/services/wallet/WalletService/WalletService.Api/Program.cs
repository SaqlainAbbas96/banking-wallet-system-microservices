using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Metrics;
using Serilog;
using System.Text;
using WalletService.Api.Endpoints;
using WalletService.Api.Infrastructure.Correlation;
using WalletService.Application;
using WalletService.Infrastructure;
using WalletService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Serilog configuration
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .Enrich.WithEnvironmentName()
        .Enrich.WithProcessId()
        .Enrich.WithThreadId();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Wallet Service API",
        Version = "v1",
        Description = "Banking Wallet System - Wallet Service"
    });
});

// Health Checks
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("WalletDb")!);

// OpenTelemetry Metrics
builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddPrometheusExporter();
    });

// Dependency Injection
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

// Authentication
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)
            ),

            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Database Migration
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<WalletDbContext>();
    dbContext.Database.Migrate();
}

// Middleware
app.UseMiddleware<CorrelationIdMiddleware>();

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// Map endpoints
app.MapHealthChecks("/health");
app.MapPrometheusScrapingEndpoint();

app.MapWalletEndpoints();

app.Run();
