using IndoeNaviAPI.Models.Statistic;
using MongoDB.Bson;

namespace IndoeNaviAPI.Services;

public interface IStatisticService
{
	Task<List<PathSession>> GetPathSessions();
	Task IncrementPathSessionToday();
    Task<List<ActiveUser>> GetActiveUsers();
    Task IncrementActiveUsersToday();
    Task<List<DestinationVisit>> GetDestinationVisits();
    Task IncrementDestinationVisits(string destination);    
    Task<List<UsedSensor>> GetUsedSensors();
    Task IncrementUsedSensors(string sensorName);
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

    public async Task IncrementPathSessionToday()
    {
        string collectionName = "pathSessions";
        // Find path session for today
        PathSession pathsession = await mongoDBService.GetFirstByKey<PathSession, DateTimeOffset>("Date", DateTimeOffset.Now.Date);
        // If not exist then create one
        if (pathsession == null)
        {
            pathsession = new PathSession { Id= ObjectId.Empty, Date = DateTimeOffset.Now.Date, Count = 1 };
            await mongoDBService.Insert(pathsession);
            return;
        }

        // Increment the counter field
        await mongoDBService.Update_IncrementField<PathSession>(pathsession.Id, "Count", 1, pathsession);
    }

    public async Task<List<ActiveUser>> GetActiveUsers()
    {
        return await mongoDBService.GetAll<ActiveUser>();
	}

    public async Task IncrementActiveUsersToday()
    {
        string collectionName = "activeUsers";
        // Find Active users for today
        ActiveUser activeUser = await mongoDBService.GetFirstByKey<ActiveUser, DateTimeOffset>("Date", DateTimeOffset.Now.Date);

        // If not exist then create one
        if (activeUser == null)
        {
            activeUser = new ActiveUser { Id = ObjectId.Empty, Date = DateTimeOffset.Now.Date, Count = 1 };
            await mongoDBService.Insert(activeUser);
            return;
        }

        // Increment the counter field
        await mongoDBService.Update_IncrementField<ActiveUser>(activeUser.Id, "Count", 1, activeUser);
    }

    public async Task<List<DestinationVisit>> GetDestinationVisits()
    {
        return await mongoDBService.GetAll<DestinationVisit>();
	}

    public async Task IncrementDestinationVisits(string destination)
    {
        string collectionName = "destinationVisit";
        // Find Active users for today
        DestinationVisit destinationVisit = await mongoDBService.GetFirstByKey<DestinationVisit, string>("Destination", destination);

        // If not exist then create one
        if (destinationVisit == null)
        {
            destinationVisit = new DestinationVisit { Id = ObjectId.Empty, Destination = destination, Count = 1 };
            await mongoDBService.Insert(destinationVisit);
            return;
        }

        // Increment the counter field
        await mongoDBService.Update_IncrementField<DestinationVisit>(destinationVisit.Id, "Count", 1, destinationVisit);
    }

    public async Task<List<UsedSensor>> GetUsedSensors()
    {
        return await mongoDBService.GetAll<UsedSensor>();
    }

    public async Task IncrementUsedSensors(string sensorName)
    {
        string collectionName = "usedSensors";
        // Find Active users for today
        UsedSensor usedSensor = await mongoDBService.GetFirstByKey<UsedSensor, string>("SensorName", sensorName);

        // If not exist then create one
        if (usedSensor == null)
        {
            usedSensor = new UsedSensor { Id = ObjectId.Empty, SensorName = sensorName, Count = 1 };
            await mongoDBService.Insert(usedSensor);
            return;
        }

        // Increment the counter field
        await mongoDBService.Update_IncrementField<UsedSensor>(usedSensor.Id, "Count", 1, usedSensor);
    }
}
