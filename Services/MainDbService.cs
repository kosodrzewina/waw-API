using Microsoft.EntityFrameworkCore;
using WawAPI.DTOs;
using WawAPI.Models;

namespace WawAPI.Services;

public interface IDatabaseService
{
    public EventDto? GetEvent(string guid);
    public List<EventDto>? GetEvents(params EventTypeEnum[] eventTypes);
    public void LikeEvent(string guid, string email, bool liked);
    public List<EventDto>? GetFavouriteEvents(string userId);
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

    public void LikeEvent(string guid, string userId, bool liked)
    {
        var @event = _context.Events.Where(e => e.Guid == guid).FirstOrDefault();
        var user = _context.Users
            .Where(u => u.Id == userId)
            .Include(e => e.Events)
            .FirstOrDefault();

        if (user is not null && @event is not null)
        {
            if (liked)
            {
                user.Events.Add(@event);
            }
            else
            {
                user.Events.Remove(@event);
            }

            _context.SaveChanges();
        }
    }

    public List<EventDto>? GetFavouriteEvents(string userId)
    {
        var user = _context.Users
            .Where(u => u.Id == userId)
            .Include(u => u.Events)
            .ThenInclude(e => e.Types)
            .Include(u => u.Events)
            .ThenInclude(e => e.Location)
            .FirstOrDefault();

        if (user is not null)
        {
            return user.Events.ToList().Select(e =>
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

        return null;
    }
}
