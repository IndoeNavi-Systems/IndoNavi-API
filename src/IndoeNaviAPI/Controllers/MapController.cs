using IndoeNaviAPI.Models;
using IndoeNaviAPI.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace IndoeNaviAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class MapController : ControllerBase
{
	// Temporary
	private static readonly Map map = new()
	{
		Area = "ZBC Ringsted",
		RouteNodes = new List<RouteNode>()
			{
				new RouteNode
				{
					X = 5,
					Y = 5,
					IsDestination= true,
					Name = "D15",
				}
},
		//ImageData = Convert.ToBase64String(System.IO.File.ReadAllBytes("Map.png")),
		SPEs = new List<SPE>
			{
				new SPE
				{
					X= 0,
					Y= 0,
					Name = "SPE 1"
				}
			}
	};

	[HttpGet]
	public Map GetMap()
	{
		return map;
	}

	[HttpPut("import")]
	public ActionResult<Map> ImportMap(string imageData)
	{
		if (!Utility.IsBase64String(imageData))
		{
			return BadRequest("Image is not encoded in base64");
		}
		map.ImageData = imageData;
		return map;
	}

	[HttpPut("nodes")]
	public Map UpdateRouteNodes(List<RouteNode> routeNodes)
	{
		map.RouteNodes = routeNodes;
		return map;
	}

	[HttpPut("spes")]
	public Map UpdateSPEs(List<SPE> spes)
	{
		map.SPEs = spes;
		return map;
	}
}