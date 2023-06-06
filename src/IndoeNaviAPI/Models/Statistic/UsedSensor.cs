using IndoeNaviAPI.Utilities;
using MongoDB.Bson;

namespace IndoeNaviAPI.Models.Statistic;

[MongoCollection("usedSensors")]
public class UsedSensor : IHasIdProp
{
    public ObjectId Id { get; set; }
	public string SensorName { get; set; }
	public int Count { get; set; }
    public string Area { get; set; }
}
