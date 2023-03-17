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
        await mongoDBService.Upsert<Map>( "maps", map.Id, map);
	}

	public async Task<Map?> GetMap(string area)
	{
		List<Map> maps = await mongoDBService.GetAllByKey<Map, string>("maps", "area", area);
		return maps.SingleOrDefault();
	}
}
