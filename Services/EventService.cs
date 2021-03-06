using System.Diagnostics;
using System.Text.RegularExpressions;
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

            var newEvents = eventFetcher.LastFetched;

            AddAddressesToEvents(newEvents);
            UpdateDb(newEvents);

            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }

    private void AddAddressesToEvents(List<Event> events) =>
        events.ForEach(e => e.Address = GetAddressFromDescription(e.Description));

    private string GetAddressFromDescription(string description)
    {
        var regex = new Regex("Miejsce: (.*)<");
        var match = regex.Match(description);

        if (match is null)
        {
            return "not found";
        }

        return match.Groups[1].ToString();
    }

    private void UpdateDb(List<Event> events)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MainDbContext>();

        // add new events to db
        context.AddRange(
            events
                .Where(
                    @event =>
                        !context.Events
                            .Join(
                                context.EventTypes,
                                eventDb => eventDb.IdEventType,
                                type => type.Id,
                                (eventDb, type) => new { IdEventType = type.Id, eventDb.Guid }
                            )
                            .Where(joined => joined.IdEventType.Equals(@event.TypeEnum.Id))
                            .Any(joined => joined.Guid.Equals(@event.Guid))
                )
                .Select(
                    @event =>
                        new Event
                        {
                            Title = @event.Title,
                            Description = @event.Description,
                            Link = @event.Link,
                            Address = @event.Address,
                            Guid = @event.Guid,
                            IsCurrent = true,
                            IdEventType = @event.TypeEnum.Id
                        }
                )
        );

        // mark appropriate events as outdated
        var guids = events.Select(@event => @event.Guid);

        context.Events
            .Where(eventDb => !guids.Contains(eventDb.Guid))
            .ToList()
            .ForEach(eventDb => eventDb.IsCurrent = false);

        context.SaveChanges();
    }
}
