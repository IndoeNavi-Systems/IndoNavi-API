using IndoeNaviAPI.Models;
using IndoeNaviAPI.Utilities;
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
        if (!Utility.IsBase64String(map.ImageData))
        {
			throw new FormatException("Image is not encoded in base64");
		}

        return mongoDBService.Upsert<Map>(map);
	}

	public async Task<Map?> GetMap(string area)
	{
        return await mongoDBService.GetFirstByKey<Map, string>("Area", area);
	}
}
