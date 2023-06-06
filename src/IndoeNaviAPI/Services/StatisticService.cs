using IndoeNaviAPI.Models;
using IndoeNaviAPI.Models.Statistic;
using MongoDB.Bson;

namespace IndoeNaviAPI.Services;

public interface IStatisticService
{
	Task<List<T>> GetAllByArea<T>(string area);
	Task IncrementPathSessionToday(string area);
    Task IncrementActiveUsersToday(string area);
    Task IncrementDestinationVisits(string destination, string area);    
    Task IncrementUsedSensors(string sensorName, string area);
    Task<bool> IsAreaExists(string area);

}

public class StatisticService : IStatisticService
{
    private readonly IMongoDBService mongoDBService;

	public StatisticService(IMongoDBService mongoDBService)
	{
		this.mongoDBService = mongoDBService;
	}

    public async Task<List<T>> GetAllByArea<T>(string area)
    {
        return await mongoDBService.GetAllByKey<T, string>("Area", area);
	}

    public async Task IncrementPathSessionToday(string area)
    {
        // Find path session for today
        PathSession pathsession = await mongoDBService.GetFirstByKey<PathSession, DateTimeOffset>("Date", DateTimeOffset.Now.Date);
        // If not exist then create one
        if (pathsession == null)
        {
            pathsession = new PathSession {Date = DateTimeOffset.Now.Date, Count = 1, Area = area };
            await mongoDBService.Insert(pathsession);
            return;
        }

        // Increment the counter field
        await mongoDBService.Update_IncrementField<PathSession>("Count", 1, pathsession);
    }

    public async Task IncrementActiveUsersToday(string area)
    {
        // Find Active users for today
        ActiveUser activeUser = await mongoDBService.GetFirstByKey<ActiveUser, DateTimeOffset>("Date", DateTimeOffset.Now.Date);

        // If not exist then create one
        if (activeUser == null)
        {
            activeUser = new ActiveUser { Date = DateTimeOffset.Now.Date, Count = 1, Area = area };
            await mongoDBService.Insert(activeUser);
            return;
        }

        // Increment the counter field
        await mongoDBService.Update_IncrementField<ActiveUser>("Count", 1, activeUser);
    }

    public async Task IncrementDestinationVisits(string destination, string area)
    {
        // Find Active users for today
        DestinationVisit destinationVisit = await mongoDBService.GetFirstByKey<DestinationVisit, string>("Destination", destination);

        // If not exist then create one
        if (destinationVisit == null)
        {
            destinationVisit = new DestinationVisit { Destination = destination, Count = 1, Area = area };
            await mongoDBService.Insert(destinationVisit);
            return;
        }

        // Increment the counter field
        await mongoDBService.Update_IncrementField<DestinationVisit>( "Count", 1, destinationVisit);
    }

    public async Task IncrementUsedSensors(string sensorName, string area)
    {
        // Find Active users for today
        UsedSensor usedSensor = await mongoDBService.GetFirstByKey<UsedSensor, string>("SensorName", sensorName);

        // If not exist then create one
        if (usedSensor == null)
        {
            usedSensor = new UsedSensor { SensorName = sensorName, Count = 1, Area = area };
            await mongoDBService.Insert(usedSensor);
            return;
        }

        // Increment the counter field
        await mongoDBService.Update_IncrementField<UsedSensor>( "Count", 1, usedSensor);
    }

        /// <summary>
    /// Check if area exits in map
    /// </summary>
    /// <param name="area"></param>
    /// <returns></returns>
    public async Task<bool> IsAreaExists(string area)
    {
        return (await mongoDBService.GetAllByKey<Map, string>("Area", area)).Count > 0;
        }
}
