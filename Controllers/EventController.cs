using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WawAPI.Services;

namespace WawAPI.Controllers;

[Route("api/events")]
[ApiController]
public class EventController : ControllerBase
{
    private readonly IDatabaseService _databaseService;

    public EventController(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    [HttpGet("by-guid")]
    public IActionResult GetEvent(string guid)
    {
        var response = _databaseService.GetEvent(guid);

        if (response == null)
        {
            return NotFound($"Event with guid \"{guid}\" does not exist");
        }

        return Ok(response);
    }

    [HttpGet("by-types")]
    public IActionResult GetEvents([FromQuery] string[] eventTypes)
    {
        List<string> notFound = new();
        var types = eventTypes
            .Select(
                t =>
                {
                    var @enum = Enumeration.Get<EventTypeEnum>(t);

                    if (@enum == null)
                        notFound.Add(t);

                    return @enum;
                }
            )
            .ToArray();

        if (notFound.Count != 0)
        {
            return NotFound($"Following types have not been found: {string.Join(", ", notFound)}");
        }

        return Ok(_databaseService.GetEvents(types!));
    }

    [HttpGet("like-count")]
    public IActionResult GetEventLikeCount(string encodedGuid)
    {
        var guid = System.Text.Encoding.UTF8.GetString(
            Convert.FromBase64String(encodedGuid)
        );
        var likeCount = _databaseService.GetEventLikeCount(guid);

        return Ok(likeCount);
    }

    [HttpPut("like")]
    [Authorize]
    public IActionResult LikeEvent(string encodedGuid, bool liked)
    {
        var guid = System.Text.Encoding.UTF8.GetString(
            Convert.FromBase64String(encodedGuid)
        );
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is not null)
        {
            _databaseService.LikeEvent(guid, userId, liked);
            return Ok();
        }

        return BadRequest();
    }

    [HttpGet("favourites")]
    [Authorize]
    public IActionResult GetFavouriteEvents()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is not null)
        {
            var events = _databaseService.GetFavouriteEvents(userId);

            if (events is not null)
            {
                return Ok(events);
            }
        }

        return BadRequest();
    }
}
