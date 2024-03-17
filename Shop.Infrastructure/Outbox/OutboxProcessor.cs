using Newtonsoft.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Shop.Infrastructure.Outbox;

using Data;

public class OutboxProcessor : BackgroundService
{
    private readonly ILogger<OutboxProcessor> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly JsonSerializerSettings _jsonSettings;
    private readonly TimeSpan _delay = TimeSpan.FromSeconds(60);

    public OutboxProcessor(ILogger<OutboxProcessor> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _jsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ProcessOutboxMessagesAsync(stoppingToken);

            await Task.Delay(_delay, stoppingToken);
        }
    }

    private async Task ProcessOutboxMessagesAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

        var messages = await dbContext.OutboxMessages
            .Where(m => m.Published == null)
            .ToListAsync(stoppingToken);

        foreach (var message in messages)
        {
            try
            {
                var domainEvent = JsonConvert.DeserializeObject<INotification>(message.Data, _jsonSettings);

                await publisher.Publish(domainEvent!, stoppingToken);
                
                message.Published = DateTime.UtcNow;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
            }
        }
        
        await dbContext.SaveChangesAsync(stoppingToken);
    }
}