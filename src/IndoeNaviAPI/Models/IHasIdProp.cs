using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ThirdParty.Json.LitJson;

namespace IndoeNaviAPI.Models;

public interface IHasIdProp
{
    [BsonId]
    Guid Id { get; set; }
}
