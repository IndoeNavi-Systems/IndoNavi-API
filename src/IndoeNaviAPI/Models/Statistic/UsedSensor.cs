using MongoDB.Bson;

namespace IndoeNaviAPI.Models.Statistic;

public class UsedSensor : IHasIdProp
{
    public ObjectId Id { get; set; }
	public string SensorName { get; set; }
	public int Count { get; set; }
}
