using IndoeNaviAPI.Utilities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using ThirdParty.Json.LitJson;

namespace IndoeNaviAPI.Models;

[MongoCollection("maps")]
public class Map : IHasIdProp
{
    [BsonId]
    public Guid Id { get; set; }
    public string Area { get; set; }
	public double MeterPerPixel { get; set; }
	public string ImageData { get; set; } // Endcoded in base64
	public List<RouteNode> RouteNodes { get; set; }

	[JsonPropertyName("spes")]
	public List<SPE> SPEs { get; set; }
}
