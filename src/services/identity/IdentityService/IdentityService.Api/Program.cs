using IdentityService.Api;
using IdentityService.Api.Endpoints;
using Microsoft.EntityFrameworkCore;
using Serilog;

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

// Add services (DI)
builder.Services.AddIdentityDependencies(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Identity Service API",
        Version = "v1",
        Description = "Banking Wallet System - Identity Service"
    });
});

var app = builder.Build();

// Middleware
app.UseSerilogRequestLogging();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

// Map endpoints
app.MapRegisterEndpoints();
app.MapLoginEndpoints();

app.Run();
