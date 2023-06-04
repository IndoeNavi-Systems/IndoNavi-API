using MongoDB.Bson;

namespace IndoeNaviAPI.Models;

public interface IHasIdProp
{
	ObjectId Id { get; set; }
}
