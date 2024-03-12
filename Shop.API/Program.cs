using Shop.API;
using Shop.Application;
using Shop.Infrastructure;
using Shop.Infrastructure.Data;
using Shop.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Register services for each layer.

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddApiServices();

builder.WebHost.UseUrls(Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "http://*:8003");

var app = builder.Build();

// Recreate the database.

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<AuthorizationExceptionMiddleware>();
app.UseMiddleware<ValidationExceptionMiddleware>();
app.UseMiddleware<UnhandledExceptionMiddleware>();

app.MapControllers();

app.Run();