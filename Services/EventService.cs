using Geocoding.Google;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.RegularExpressions;
using WawAPI.Models;
using Location = WawAPI.Models.Location;

namespace WawAPI.Services;

public class EventService : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly MainDbContext _context;
    private readonly GoogleGeocoder _geocoder;

    public EventService(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
    {
        var scope = serviceScopeFactory.CreateScope();

        _configuration = configuration;
        _context = scope.ServiceProvider.GetRequiredService<MainDbContext>();
        _geocoder = new GoogleGeocoder()
        {
            ApiKey = _configuration.GetValue<string>("ApiKeys:GoogleApiKey")
        };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var eventFetcher = new EventFetcher(Enumeration.GetAll<EventTypeEnum>().ToArray());

        while (!stoppingToken.IsCancellationRequested)
        {
            Debug.WriteLine(LogLevel.Information, $"{DateTime.Now} Fetching all events...");
            await eventFetcher.Fetch();
            Debug.WriteLine(LogLevel.Information, $"{DateTime.Now} All events have been fetched");

            var fetchedEvents = eventFetcher.LastFetched;
            var newEvents = await SelectAndPrepareNewEvents(fetchedEvents);

            UpdateDb(fetchedEvents, newEvents);
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

    private async Task<List<Event>> SelectAndPrepareNewEvents(List<Event> events)
    {
        var newEvents = new List<Event>();

        foreach (var @event in events)
        {
            var typeIds = @event.TypeEnums.Select(t => t.Id);
            var types = _context.EventTypes.Where(t => typeIds.Contains(t.Id)).ToList();
            var eventsDb = _context.Events
                .Include(e => e.Types).ToList()
                .Where(e => e.Types.Intersect(types).Any()).ToList();

            if (!eventsDb.Any(e => e.Guid == @event.Guid))
            {
                @event.IsCurrent = true;
                @event.Types = types;
                newEvents.Add(@event);
            }
        }

        AddAddressesToEvents(newEvents);
        await AddLocationsToEvents(newEvents);

        return newEvents;
    }

    private async Task AddLocationsToEvents(List<Event> events)
    {
        foreach (var @event in events)
        {
            Debug.WriteLine($"Getting location for {@event.Address}...");

            var addresses = await _geocoder.GeocodeAsync(@event.Address);
            var address = addresses.FirstOrDefault();

            if (address is not null)
            {
                @event.Location = new Location
                {
                    Latitude = address.Coordinates.Latitude,
                    Longitude = address.Coordinates.Longitude
                };
            }
        }
    }

    private void UpdateDb(List<Event> allEvents, List<Event> newEvents)
    {
        _context.AddRange(newEvents);
        CheckEventsValidity(allEvents);
        _context.SaveChanges();
    }

    private void CheckEventsValidity(List<Event> events)
    {
        _context.Events.Where(eDb => eDb.IsCurrent).ToList().ForEach(eDb =>
        {
            if (!events.Any(e => e.Guid == eDb.Guid))
            {
                eDb.IsCurrent = false;
            }
        });
    }
}
