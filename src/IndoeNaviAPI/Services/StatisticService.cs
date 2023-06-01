using IndoeNaviAPI.Models;
using IndoeNaviAPI.Models.Statistic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace IndoeNaviAPI.Services;

public interface IStatisticService
{
	Task<List<PathSession>> GetPathSessions();
	Task IncrementPathSessionToday();
    Task<List<ActiveUsers>> GetActiveUsers();
    Task IncrementActiveUsersToday();
}

public class StatisticService : IStatisticService
{
    private readonly IMongoDBService mongoDBService;

	public StatisticService(IMongoDBService mongoDBService)
	{
		this.mongoDBService = mongoDBService;
	}

	public async Task UpdateMap(Map map)
	{
        await mongoDBService.Upsert<Map>( "maps", map.Id, map);
	}

	public async Task<Map?> GetMap(string area)
	{
		List<Map> maps = await mongoDBService.GetAllByKey<Map, string>("maps", "Area", area);
		return maps.SingleOrDefault();
	}

    public async Task<List<PathSession>> GetPathSessions()
    {
        List<PathSession> pathSessions = await mongoDBService.GetAll<PathSession>("pathSessions");
        return pathSessions;
    }

    public async Task IncrementPathSessionToday()
    {
        // Find path session for today
        PathSession pathsession = await mongoDBService.GetFirstByKey<PathSession, DateTimeOffset>("pathSessions", "Date", DateTimeOffset.Now.Date);
        // If not exist then create one
        if (pathsession == null)
        {
            pathsession = new PathSession { Id= ObjectId.Empty, Date = DateTimeOffset.Now.Date, Count = 1 };
            await mongoDBService.Insert(pathsession, "pathSessions");
            return;
        }


        // Increment the counter field
        await mongoDBService.Update<PathSession>("pathSessions", pathsession.Id,
            Builders<PathSession>.Update.Inc(a => a.Count, 1), pathsession);
        return;
    }

    public async Task<List<ActiveUsers>> GetActiveUsers()
    {
        List<ActiveUsers> activeUsers = await mongoDBService.GetAll<ActiveUsers>("activeUsers");
        return activeUsers;
    }

    public async Task IncrementActiveUsersToday()
    {
        // Find Active users for today
        ActiveUsers activeUser = await mongoDBService.GetFirstByKey<ActiveUsers, DateTimeOffset>("activeUsers", "Date", DateTimeOffset.Now.Date);

        // If not exist then create one
        if (activeUser == null)
        {
            activeUser = new ActiveUsers { Id = ObjectId.Empty, Date = DateTimeOffset.Now.Date, Count = 1 };
            await mongoDBService.Insert(activeUser, "activeUsers");
            return;
        }


        // Increment the counter field
        await mongoDBService.Update<ActiveUsers>("activeUsers", activeUser.Id,
            Builders<ActiveUsers>.Update.Inc(a => a.Count, 1), activeUser);
        return;
    }
}
