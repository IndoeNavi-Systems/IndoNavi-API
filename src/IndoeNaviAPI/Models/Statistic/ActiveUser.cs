using IndoeNaviAPI.Utilities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IndoeNaviAPI.Models.Statistic;

[MongoCollection("activeUsers")]
public class ActiveUser : IHasIdProp
{
    [BsonId]
    public Guid Id { get; set; }
    public DateTimeOffset Date { get; set; }
    public int Count { get; set; }
    public string Area { get; set; }
}
