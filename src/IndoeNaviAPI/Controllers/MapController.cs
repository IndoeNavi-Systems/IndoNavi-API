using IndoeNaviAPI.Models;
using IndoeNaviAPI.Services;
using IndoeNaviAPI.Utilities;
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
	public IActionResult UpdateMap(Map map)
	{
		if (!Utility.IsBase64String(map.ImageData))
		{
			return BadRequest("Image is not encoded in base64");
		}
		mapService.UpdateMap(map);
		return Ok();
	}
}