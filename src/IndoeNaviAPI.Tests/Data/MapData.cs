using IndoeNaviAPI.Models;

namespace IndoeNaviAPI.Tests.Data;

public class MapData
{
	public static Map Create()
	{
		return new Map
		{
			Area = "test-map",
			ImageData = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNk+A8AAQUBAScY42YAAAAASUVORK5CYII=",
			MeterPerPixel = 1,
			SPEs = new List<SPE>(),
			RouteNodes = new List<RouteNode>(),
		};
	}
}
