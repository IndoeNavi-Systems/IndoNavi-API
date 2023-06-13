using MongoDB.Bson.Serialization.Attributes;

namespace IndoeNaviAPI.Models;

public interface IHasIdProp
{
    [BsonId]
    Guid Id { get; set; }
}
