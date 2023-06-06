using IndoeNaviAPI.Utilities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IndoeNaviAPI.Models.Statistic;

[MongoCollection("usedSensors")]
public class UsedSensor : IHasIdProp
{
	[BsonId]
    public Guid Id { get; set; }
	public string SensorName { get; set; }
	public int Count { get; set; }
    public string Area { get; set; }
}
