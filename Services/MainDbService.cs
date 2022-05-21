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
            .Where(e => e.Guid.Equals(guid))
            .Join(
                _context.EventTypes,
                e => e.IdEventType,
                t => t.Id,
                (e, t) =>
                    new EventDto
                    {
                        Title = e.Title,
                        Description = e.Description,
                        Link = e.Link,
                        Guid = e.Guid,
                        Type = t.Name
                    }
            )
            .FirstOrDefault();
    }

    public List<EventDto> GetEvents(params EventTypeEnum[] eventTypes)
    {
        return _context.Events
            .Join(
                _context.EventTypes,
                e => e.IdEventType,
                t => t.Id,
                (e, t) =>
                    new { e.Title, e.Description, e.Link, e.Guid, TypeId = t.Id, TypeName = t.Name }
            )
            .ToList()
            .Where(e => eventTypes.Any(t => t.Id.Equals(e.TypeId)))
            .Select(
                joinResult =>
                    new EventDto
                    {
                        Title = joinResult.Title,
                        Description = joinResult.Description,
                        Link = joinResult.Link,
                        Guid = joinResult.Guid,
                        Type = joinResult.TypeName
                    }
            )
            .ToList();
    }

    public void UpdateDb(List<Event> events)
    {
        // add new events to db
        _context.AddRange(
            events
                .Where(
                    @event =>
                        !_context.Events
                            .Join(
                                _context.EventTypes,
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
                            Guid = @event.Guid,
                            IsCurrent = true,
                            IdEventType = @event.TypeEnum.Id
                        }
                )
        );

        // mark appropriate events as outdated
        _context.Events
            .Where(
                @event =>
                    !events
                        .Join(
                            _context.EventTypes,
                            eventDb => eventDb.IdEventType,
                            type => type.Id,
                            (eventDb, type) => new { IdEventType = type.Id, eventDb.Guid }
                        )
                        .Where(joined => joined.IdEventType.Equals(@event.TypeEnum.Id))
                        .Any(joined => joined.Guid.Equals(@event.Guid))
            )
            .ToList()
            .ForEach(@event => @event.IsCurrent = false);

        _context.SaveChanges();
    }
}
