using IndoeNaviAPI.Utilities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IndoeNaviAPI.Models.Statistic;

[MongoCollection("usedSensors")]
public class UsedSensor : IHasIdProp, IHasAreaProp, IAmNameValueStatistic
{
	[BsonId]
    public Guid Id { get; set; }
    [BsonElement("SensorName")]
    public string Name { get; set; }
	public int Count { get; set; }
    public string Area { get; set; }
}
