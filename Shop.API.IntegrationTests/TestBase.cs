using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;

namespace Shop.API.IntegrationTests;

using Infrastructure.Authentication;
using Application.Common.Interfaces;

public abstract class TestBase : IAsyncLifetime
{
    private readonly Func<Task> _resetDatabase;
    private readonly IJwtProvider _jwtProvider;
    
    protected HttpClient Client { get; set; }

    protected TestBase(ShopApiFactory factory)
    {
        Client = factory.CreateClient();
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