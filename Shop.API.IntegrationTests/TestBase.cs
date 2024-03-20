using Microsoft.Extensions.DependencyInjection;

namespace Shop.API.IntegrationTests;

using Application.Common.Interfaces;

[Collection("TestCollection")]
public abstract class TestBase : IAsyncLifetime
{
    private readonly Func<Task> _resetDatabase;
    private readonly IJwtProvider _jwtProvider;
    
    protected HttpClient Client { get; }
    protected TestDataFactory DataFactory { get; }

    protected TestBase(ShopApiFactory factory)
    {
        Client = factory.CreateClient();
        DataFactory = new TestDataFactory(factory.Context);
        _resetDatabase = factory.ResetDatabaseAsync;
        _jwtProvider = factory.Services.GetRequiredService<IJwtProvider>();
    }

    protected void EnableAuthentication(string role)
    {
        var token = _jwtProvider.GenerateToken(Guid.NewGuid(), "user@test.com", new List<string> { role });

        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    protected void DisableAuthentication()
    {
        Client.DefaultRequestHeaders.Authorization = null;
    }
    
    public async Task InitializeAsync()
    {
        await _resetDatabase();
    }

    public async Task DisposeAsync()
    {
        await Task.CompletedTask;
    }
}