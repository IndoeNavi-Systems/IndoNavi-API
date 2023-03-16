namespace IndoeNaviAPI.Models;

public class Map
{
	public string Area { get; set; }
	public string ImageData { get; set; } // Endcoded in base64
	public List<RouteNode> RouteNodes { get; set; }
	public List<SPE> SPEs { get; set; }
}
