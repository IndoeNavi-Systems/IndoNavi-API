using IndoeNaviAPI.Utilities;
using MongoDB.Bson.Serialization.Attributes;

namespace IndoeNaviAPI.Models.Statistic;

[MongoCollection("DestinationVisits")]
public class DestinationVisit : IHasIdProp, IHasAreaProp, IAmNameValueStatistic
{
    [BsonId]
    public Guid Id { get; set; }
    public string Name { get; set; }
	public int Count { get; set; }
    public string Area { get; set; }
}
