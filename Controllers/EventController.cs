using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WawAPI.Services;

namespace WawAPI.Controllers;

[Route("api/events")]
[ApiController]
public class EventController : ControllerBase
{
    private readonly IDatabaseService _databaseService;

    public EventController(IDatabaseService databaseService) => _databaseService = databaseService;

    [HttpGet]
    public IActionResult GetEvent(string guid)
    {
        var response = _databaseService.GetEvent(guid);

        if (response == null)
        {
            return NotFound($"Event with guid: {guid} does not exist");
        }

        return Ok(response);
    }
}
