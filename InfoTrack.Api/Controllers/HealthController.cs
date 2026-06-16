using Microsoft.AspNetCore.Mvc;

namespace InfoTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController() : ControllerBase
{
    [HttpGet]
    public IActionResult GetStatus()
    {
        return Ok("Live");
    }
}