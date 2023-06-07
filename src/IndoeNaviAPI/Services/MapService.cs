using IndoeNaviAPI.Models;
using MongoDB.Driver;

namespace IndoeNaviAPI.Services;

public interface IMapService
{
	Task<Map?> GetMap(string area);
	Task UpsertMap(Map map);
}

public class MapService : IMapService
{
    private readonly IMongoDBService mongoDBService;

	public MapService(IMongoDBService mongoDBService)
	{
		this.mongoDBService = mongoDBService;
	}

	public Task UpsertMap(Map map)
	{
 		return mongoDBService.Upsert<Map>(map);
	}

	public async Task<Map?> GetMap(string area)
	{
        return (await mongoDBService.GetAllByKey<Map, string>( "Area", area)).SingleOrDefault();
	}
}
