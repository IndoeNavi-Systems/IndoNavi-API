namespace IndoeNaviAPI.Models;

public class RouteNode
{
	public double X { get; set; }
	public double Y { get; set; }
	public bool IsDestination { get; set; }
	public List<RouteNode> RouteNodes { get; set; }
	public string Name { get; set; }
	
}
