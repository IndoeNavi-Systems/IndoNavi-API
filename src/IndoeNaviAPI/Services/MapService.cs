using IndoeNaviAPI.Models;

namespace IndoeNaviAPI.Services;

public interface IMapService
{
	Task<Map?> GetMap(string area);
	Task UpdateMap(Map map);
}

public class MapService : IMapService
{
	private readonly IMongoDBService mongoDBService;

	public MapService(IMongoDBService mongoDBService)
	{
		this.mongoDBService = mongoDBService;
	}

	public async Task UpdateMap(Map map)
	{
		Map mapFromDb = await mongoDBService.GetFirstByKey<Map, string>("maps", "Area", map.Area);
		map.Id = mapFromDb.Id;
		await mongoDBService.Upsert<Map>("maps", mapFromDb.Id, map);
	}

	public async Task<Map?> GetMap(string area)
	{
		return (await mongoDBService.GetAllByKey<Map, string>("maps", "Area", area)).SingleOrDefault();
	}
}
