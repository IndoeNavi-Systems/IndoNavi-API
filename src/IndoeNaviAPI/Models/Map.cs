using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace IndoeNaviAPI.Models;

public class Map : IHasIdProp
{
	public ObjectId Id { get; set; }
    public string Area { get; set; }
	public double MeterPerPixel { get; set; }
	public string ImageData { get; set; } // Endcoded in base64
	public List<RouteNode> RouteNodes { get; set; }

	[JsonPropertyName("spes")]
	public List<SPE> SPEs { get; set; }
}
