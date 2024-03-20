using System.Data.Common;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
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

    public ApplicationDbContext Context { get; private set; } = default!;
    
    public ShopApiFactory()
    {
        _container = new PostgreSqlBuilder().Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            if (descriptor != null) services.Remove(descriptor);
            
            services.AddDbContext<ApplicationDbContext>((options) =>
            {
                options.UseNpgsql(_container.GetConnectionString());
                // options.AddInterceptors(provider.GetRequiredService<OutboxInterceptor>());
            });

            using var scope = services.BuildServiceProvider().CreateScope();
            
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            context.Database.EnsureCreated();
        });
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_connection);
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        Context = Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();

        _connection = Context.Database.GetDbConnection();
        
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
        
        await _connection.DisposeAsync();
    }
}