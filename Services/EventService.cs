using Microsoft.EntityFrameworkCore;
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

        foreach (var @event in events)
        {
            var typeIds = @event.TypeEnums.Select(t => t.Id);
            var types = context.EventTypes.Where(t => typeIds.Contains(t.Id)).ToList();
            var eventsDb = context.Events
                .Include(e => e.Types).ToList()
                .Where(e => e.Types.Intersect(types).Any()).ToList();

            if (!eventsDb.Any(e => e.Guid == @event.Guid))
            {
                @event.IsCurrent = true;
                @event.Types = types;
                context.Add(@event);
            }
        }

        // mark appropriate events as outdated
        context.Events.Where(eDb => eDb.IsCurrent).ToList().ForEach(eDb =>
        {
            if (!events.Any(e => e.Guid == eDb.Guid))
            {
                eDb.IsCurrent = false;
            }
        });

        context.SaveChanges();
    }
}
