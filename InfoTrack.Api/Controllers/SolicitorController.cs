using InfoTrack.Application.Interfaces;
using InfoTrack.Domain.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace InfoTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SolicitorsController(ISolicitorSearch search) : ControllerBase
{
    [HttpPost]
    [Route("getByLocations")]
    public async Task<IActionResult> GetByLocations([FromBody] IEnumerable<string> locations)
    {
        if (locations.IsNullOrEmpty()) 
        {
            return BadRequest("At least one location is required.");
        }
        
        var result = await search.SearchByLocationsAsync(locations, CancellationToken.None);
        return Ok(result);
    }
}