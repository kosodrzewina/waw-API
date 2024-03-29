﻿using Geocoding.Google;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using WawAPI.Models;
using Location = WawAPI.Models.Location;

namespace WawAPI.Services;

public class EventService : BackgroundService
{
    private readonly ILogger<EventService> _logger;
    private readonly IConfiguration _configuration;
    private readonly MainDbContext _context;
    private readonly GoogleGeocoder _geocoder;

    public EventService(
        IServiceScopeFactory serviceScopeFactory,
        IConfiguration configuration,
        ILogger<EventService> logger
    )
    {
        var scope = serviceScopeFactory.CreateScope();

        _configuration = configuration;
        _context = scope.ServiceProvider.GetRequiredService<MainDbContext>();
        _geocoder = new GoogleGeocoder()
        {
            ApiKey = _configuration.GetValue<string>("ApiKeys:GoogleApiKey")
        };
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var eventFetcher = new EventFetcher(Enumeration.GetAll<EventTypeEnum>().ToArray());

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation($"{DateTime.Now} Fetching all events...");
            await eventFetcher.FetchAsync();
            _logger.LogInformation($"{DateTime.Now} All events have been fetched");

            var fetchedEvents = eventFetcher.LastFetched;
            var newEvents = await SelectAndPrepareNewEventsAsync(fetchedEvents);

            UpdateDb(fetchedEvents, newEvents);
            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }

    private async Task<List<Event>> SelectAndPrepareNewEventsAsync(List<Event> events)
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

        AddImagesToEvents(newEvents);
        AddAddressesToEvents(newEvents);
        await AddLocationsToEventsAsync(newEvents);

        return newEvents;
    }

    private void AddImagesToEvents(List<Event> events)
    {
        foreach (var @event in events)
        {
            _logger.LogInformation($"Getting image for {@event.Title}");

            var regex = new Regex("<img src=\"([^\"]*)\"");
            var match = regex.Match(@event.Description);

            @event.Image = match is not null ?
                match.Groups[1].ToString() :
                "not found";
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

    private async Task AddLocationsToEventsAsync(List<Event> events)
    {
        foreach (var @event in events)
        {
            _logger.LogInformation($"Getting location for {@event.Address}...");

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
