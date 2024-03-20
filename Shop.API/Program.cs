using Shop.API;
using Shop.API.Middleware;
using Shop.Application;
using Shop.Infrastructure;
using Shop.Infrastructure.Data;
using Shop.Infrastructure.Identity;

var builder = WebApplication.CreateBuilder(args);

// Register services for each layer.

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddApiServices();

builder.WebHost.UseUrls(Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "http://*:8000");

var app = builder.Build();

// Recreate the database.

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    // db.Database.EnsureDeleted();
    db.Database.EnsureCreated();

    var userSeeder = scope.ServiceProvider.GetRequiredService<UserSeeder>();

    await userSeeder.SeedAsync();
}

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<UnhandledExceptionMiddleware>();
app.UseMiddleware<AuthorizationExceptionMiddleware>();
app.UseMiddleware<ValidationExceptionMiddleware>();

app.MapControllers();

app.Run();

public partial class Program { }