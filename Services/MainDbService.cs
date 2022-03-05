using WawAPI.DTOs;
using WawAPI.Models;

namespace WawAPI.Services;

public interface IDatabaseService
{
    public EventDto? GetEvent(string guid);
    public IDictionary<string, List<EventDto>>? GetEvents(params EventTypeEnum[] eventTypes);
}

public class MainDbService : IDatabaseService
{
    private readonly MainDbContext _context = new();

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
                        Type = t.Name
                    }
            )
            .FirstOrDefault();
    }

    public IDictionary<string, List<EventDto>> GetEvents(params EventTypeEnum[] eventTypes)
    {
        return _context.Events
            .Join(
                _context.EventTypes,
                e => e.IdEventType,
                t => t.Id,
                (e, t) => new { e.Title, e.Description, e.Link, TypeId = t.Id, TypeName = t.Name }
            )
            .ToList()
            .Where(e => eventTypes.Any(t => t.Id.Equals(e.TypeId)))
            .Select(
                e =>
                    new EventDto
                    {
                        Title = e.Title,
                        Description = e.Description,
                        Link = e.Link,
                        Type = e.TypeName
                    }
            )
            .GroupBy(e => e.Type)
            .ToDictionary(g => g.Key, g => g.ToList());
    }
}
