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
                (e, t) => new EventDto
                {
                    Title = e.Title,
                    Description = e.Description,
                    Link = e.Link,
                    Type = t.Name
                }
            );

        if (query.Any())
        {
            return query.First();
        }

        return null;
    }
}
