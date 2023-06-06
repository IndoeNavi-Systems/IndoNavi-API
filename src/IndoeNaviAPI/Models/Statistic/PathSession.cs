using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IndoeNaviAPI.Models.Statistic;

public class PathSession : IHasIdProp
{
    [BsonId]
    public Guid Id { get; set; }
    public DateTimeOffset Date { get; set; }
    public int Count { get; set; }
}
