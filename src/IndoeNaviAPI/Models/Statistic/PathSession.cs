using IndoeNaviAPI.Utilities;
using MongoDB.Bson;

namespace IndoeNaviAPI.Models.Statistic;

[MongoCollection("pathSessions")]
public class PathSession : IHasIdProp
{
    public ObjectId Id { get; set; }
    public DateTimeOffset Date { get; set; }
    public int Count { get; set; }
}
