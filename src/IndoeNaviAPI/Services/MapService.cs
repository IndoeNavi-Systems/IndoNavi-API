using IndoeNaviAPI.Models;

namespace IndoeNaviAPI.Services;

public class MapService
{
    private IMongoDBService mongoDBService;

	public MapService(IMongoDBService mongoDBService)
	{
		this.mongoDBService = mongoDBService;
	}

	public async Task UpdateMap(string mapData, string area)
	{
		// Check if any map exists with the area name
		Map map = await GetMap(area);
		map.Area = area;
		map.ImageData = mapData;

        await mongoDBService.Upsert<Map>( "maps", map.Id, map);
	}

	public async Task<Map> GetMap(string area)
	{
		List<Map> maps = await mongoDBService.GetAllByKey<Map, string>("maps", "area", area);
		Map map = maps.Any() ? maps.First() : new();
		return map;
	}
}
