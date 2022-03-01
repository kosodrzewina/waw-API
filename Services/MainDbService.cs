using WawAPI.DTOs;
using WawAPI.Models;

namespace WawAPI.Services;

public interface IDatabaseService
{
    public EventDto? GetEvent(string guid);
}

public class MainDbService : IDatabaseService
{
    private readonly MainDbContext _context = new();

    public EventDto? GetEvent(string guid)
    {
        var query = _context.Events
            .Where(e => e.Guid.Equals(guid))
            .Join(
                _context.EventTypes,
                e => e.IdEventType,
                t => t.Id,
                (e, t) => new
                {
                    e.Title,
                    e.Description,
                    e.Link,
                    EventTypeName = t.Name
                }
            );

        if (query.Any())
        {
            var @event = query.First();

            return new EventDto
            {
                Title = @event.Title,
                Description = @event.Description,
                Link = @event.Link,
                Type = @event.EventTypeName
            };
        }

        return null;
    }
}
