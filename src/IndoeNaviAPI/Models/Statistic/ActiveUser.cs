using IndoeNaviAPI.Utilities;
using MongoDB.Bson.Serialization.Attributes;

namespace IndoeNaviAPI.Models.Statistic;

[MongoCollection("activeUsers")]
public class ActiveUser : IAmDateValueStatistic, IHasAreaProp
{
    [BsonId]
    public Guid Id { get; set; }
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Date { get; set; }
    public int Count { get; set; }
    public string Area { get; set; }
}
