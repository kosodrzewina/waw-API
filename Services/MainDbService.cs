using Microsoft.EntityFrameworkCore;
using WawAPI.DTOs;
using WawAPI.Models;

namespace WawAPI.Services;

public interface IDatabaseService
{
    public EventDto? GetEvent(string guid);
    public List<EventDto>? GetEvents(params EventTypeEnum[] eventTypes);
    public void UpdateDb(List<Event> events);
}

public class MainDbService : IDatabaseService
{
    private readonly MainDbContext _context;

    public MainDbService(MainDbContext context)
    {
        _context = context;
    }

    public EventDto? GetEvent(string guid)
    {
        return _context.Events
            .Where(e => e.Guid == guid)
            .Select(
                e =>
                    new EventDto
                    {
                        Title = e.Title,
                        Description = e.Description,
                        Link = e.Link,
                        Address = e.Address,
                        Guid = e.Guid,
                        Types = e.Types.Select(t => t.Name).ToList()
                    }
            )
            .FirstOrDefault();
    }

    public List<EventDto> GetEvents(params EventTypeEnum[] eventTypes)
    {
        var types = _context.EventTypes.ToList()
            .Where(t1 => eventTypes.Any(t2 => t2.Id == t1.Id)).ToList();

        return _context.Events
            .Include(e => e.Types)
            .ToList()
            .Where(e => e.Types.Intersect(types).Any())
            .Select(e => new EventDto
            {
                Title = e.Title,
                Description = e.Description,
                Link = e.Link,
                Address = e.Address,
                Guid = e.Guid,
                Types = e.Types.Select(t => t.Name).ToList()
            }
            ).ToList();
    }

    public void UpdateDb(List<Event> events)
    {
        foreach (var @event in events)
        {
            // all of the events from @event categories
            var eventsDb = _context.Events
                .Where(e =>
                    e.Types
                    .Select(t => t.Name)
                    .Intersect(@event.Types.Select(t => t.Name)).Any()
                ).ToList();

            if (!eventsDb.Any(e => e.Guid == @event.Guid))
            {
                _context.Add(@event);
            }
        }

        // mark appropriate events as outdated
        _context.Events.Where(eDb => eDb.IsCurrent).ToList().ForEach(eDb =>
        {
            if (!events.Any(e => e.Guid == eDb.Guid))
            {
                eDb.IsCurrent = false;
            }
        });

        _context.SaveChanges();
    }
}
