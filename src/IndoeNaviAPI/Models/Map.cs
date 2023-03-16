using System.Drawing;

namespace IndoeNaviAPI.Models;

public class Map
{
	public string Area { get; set; }
	public Bitmap Image { get; set; } 
	public List<RouteNode> RouteNodes { get; set; }
	public List<SPE> SPEs { get; set; }
}
