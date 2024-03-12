using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Shop.Infrastructure.Outbox;

using Domain.Primitives;

public class OutboxInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (eventData.Context != null)
        {
            await SaveOutboxMessages(eventData.Context, cancellationToken);
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private async Task SaveOutboxMessages(DbContext context, CancellationToken cancellationToken)
    {
        var domainEntities = context.ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.Events.Any()).ToList();
        
        var domainEvents = domainEntities.SelectMany(e => e.Entity.Events).ToList();

        var outboxMessages = new List<OutboxMessage>();

        foreach (var domainEvent in domainEvents)
        {
            var message = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = domainEvent.GetType().Name,
                Data = JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                })
            };
            
            outboxMessages.Add(message);
        }
        
        await context.AddRangeAsync(outboxMessages, cancellationToken);
        
        domainEntities.ForEach(e => e.Entity.ClearDomainEvents());
    }
}