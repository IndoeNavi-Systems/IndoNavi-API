using MongoDB.Bson;

namespace IndoeNaviAPI.Models.Statistic;

public class DestinationVisit : IHasIdProp
{
	public ObjectId Id { get; set; }
	public string Destination { get; set; }
	public int Count { get; set; }
}
