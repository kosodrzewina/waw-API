using Microsoft.EntityFrameworkCore;
using WawAPI.DTOs;
using WawAPI.Models;

namespace WawAPI.Services;

public interface IDatabaseService
{
    public EventDto? GetEvent(string guid);
    public List<EventDto>? GetEvents(params EventTypeEnum[] eventTypes);
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
            .Include(e => e.Location)
            .ToList()
            .Where(e => e.Guid == guid)
            .Select(
                e =>
                {
                    var eventDto = new EventDto
                    {
                        Title = e.Title,
                        Description = e.Description,
                        Link = e.Link,
                        Address = e.Address,
                        Image = e.Image,
                        Guid = e.Guid,
                        Types = e.Types.Select(t => t.Name).ToList()
                    };

                    if (e.Location is not null)
                    {
                        eventDto.Location = new LocationDto
                        {
                            Latitude = e.Location.Latitude,
                            Longitude = e.Location.Longitude
                        };
                    }

                    return eventDto;
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
            .Include(e => e.Location)
            .ToList()
            .Where(e => e.Types.Intersect(types).Any() && e.IsCurrent)
            .Select(e =>
                {
                    var eventDto = new EventDto
                    {
                        Title = e.Title,
                        Description = e.Description,
                        Link = e.Link,
                        Address = e.Address,
                        Image = e.Image,
                        Guid = e.Guid,
                        Types = e.Types.Select(t => t.Name).ToList()
                    };

                    if (e.Location is not null)
                    {
                        eventDto.Location = new LocationDto
                        {
                            Latitude = e.Location.Latitude,
                            Longitude = e.Location.Longitude
                        };
                    }

                    return eventDto;
                }
            ).ToList();
    }
}
