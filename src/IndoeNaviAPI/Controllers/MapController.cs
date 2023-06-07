using IndoeNaviAPI.Models;
using IndoeNaviAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IndoeNaviAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class MapController : ControllerBase
{
	private readonly IMapService mapService;

	public MapController(IMapService mapService)
	{
		this.mapService = mapService;
	}

	[HttpGet]
	public async Task<ActionResult<Map>> GetMap(string area)
	{
		Map? map = await mapService.GetMap(area);
		if (map == null)
		{
			return NotFound($"No map exist on {area}");
		}
		return map;
	}

	[HttpPut]
	public async Task<IActionResult> UpsertMap(Map map)
	{
		try
		{
			await mapService.UpsertMap(map);
		}
		catch (Exception dup_ex) when (dup_ex.Message.Contains("duplicate"))
		{

			return Conflict("Duplicate key is not allowed ");
        }
		catch (FormatException format_ex) when (format_ex.Message.Contains("Image is not encoded in base64")) 
		{
			return BadRequest("Image is not encoded in base64");
		}
		// For all other exceptions we only show something bad happend
        catch (Exception e)
        {
            return BadRequest("Something bad happend...");
        }
        return Ok();
	}
}