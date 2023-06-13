using IndoeNaviAPI.Models;
using IndoeNaviAPI.Utilities;

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

	public async Task UpsertMap(Map map)
	{
		if (!EncodingChecker.IsBase64String(map.ImageData))
        {
			throw new FormatException("Image is not encoded in base64");
		}

        await mongoDBService.Upsert<Map>(map);
	}

	public async Task<Map?> GetMap(string area)
	{
        return await mongoDBService.GetFirstByKey<Map, string>("Area", area);
	}
}
