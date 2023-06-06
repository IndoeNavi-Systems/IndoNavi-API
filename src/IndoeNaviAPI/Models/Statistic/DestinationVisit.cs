using IndoeNaviAPI.Utilities;
using MongoDB.Bson;

namespace IndoeNaviAPI.Models.Statistic;

[MongoCollection("DestinationVisits")]
public class DestinationVisit : IHasIdProp
{
    public ObjectId Id { get; set; }
	public string Destination { get; set; }
	public int Count { get; set; }
    public string Area { get; set; }
}
