using MongoDB.Bson;

namespace IndoeNaviAPI.Models;

public class Map : IHasIdProp
{
	public ObjectId Id { get; set; }
    public string Area { get; set; }
	public string ImageData { get; set; } // Endcoded in base64
	public List<RouteNode> RouteNodes { get; set; }
	public List<SPE> SPEs { get; set; }
}
