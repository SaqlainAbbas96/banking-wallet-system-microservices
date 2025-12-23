using IdentityService.Api;
using IdentityService.Api.Endpoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

// Map endpoints
app.MapRegisterEndpoints();

app.Run();
