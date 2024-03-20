using System.Data.Common;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Respawn;
using Npgsql;

namespace Shop.API.IntegrationTests;

using Infrastructure.Data;
using Infrastructure.Outbox;

public class ShopApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _container;
    private DbConnection _connection = default!;
    private Respawner _respawner = default!;

    public HttpClient HttpClient { get; private set; } = default!;

    public ShopApiFactory()
    {
        _container = new PostgreSqlBuilder().Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // base.ConfigureWebHost(builder);

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            if (descriptor != null) services.Remove(descriptor);
            
            services.AddDbContext<ApplicationDbContext>((provider, options) =>
            {
                options.UseNpgsql(_container.GetConnectionString());
                // options.AddInterceptors(provider.GetRequiredService<OutboxInterceptor>());
                // options.EnableSensitiveDataLogging();
            });
        });
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_connection);
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        _connection = new NpgsqlConnection(_container.GetConnectionString());

        HttpClient = CreateClient();

        // using var scope = Services.CreateScope();
        //
        // var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        //
        // await context.Database.EnsureCreatedAsync();

        await _connection.OpenAsync();

        _respawner = await Respawner.CreateAsync(_connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = new[] { "public" }
        });
    }

    public new async Task DisposeAsync()
    {
        await _container.StopAsync();
        
        _connection.Dispose();
    }
}