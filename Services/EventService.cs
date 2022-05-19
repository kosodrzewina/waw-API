using System.Diagnostics;
using WawAPI.Models;

namespace WawAPI.Services;

public class EventService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public EventService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var eventFetcher = new EventFetcher(Enumeration.GetAll<EventTypeEnum>().ToArray());

        while (!stoppingToken.IsCancellationRequested)
        {
            Debug.WriteLine(LogLevel.Information, $"{DateTime.Now} Fetching all events...");
            await eventFetcher.Fetch();
            Debug.WriteLine(LogLevel.Information, $"{DateTime.Now} All events have been fetched");

            SaveToDb(eventFetcher.LastFetched);

            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }

    private void SaveToDb(List<Event> events)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MainDbContext>();

        context.AddRange(
            events
                .Where(
                    e =>
                        !context.Events
                            .Join(
                                context.EventTypes,
                                eDb => eDb.IdEventType,
                                t => t.Id,
                                (eDb, t) => new { IdEventType = t.Id, eDb.Guid }
                            )
                            .Where(joined => joined.IdEventType.Equals(e.TypeEnum.Id))
                            .Any(joined => joined.Guid.Equals(e.Guid))
                )
                .Select(
                    e =>
                        new Event
                        {
                            Title = e.Title,
                            Description = e.Description,
                            Link = e.Link,
                            Guid = e.Guid,
                            IdEventType = e.TypeEnum.Id
                        }
                )
        );
        context.SaveChanges();
    }
}
