using MongoDB.Bson;

namespace IndoeNaviAPI.Models.Statistic;

public class ActiveUsers : IHasIdProp
{
    public ObjectId Id { get; set; }
    public DateTimeOffset Date { get; set; }
    public int Count { get; set; }
}
