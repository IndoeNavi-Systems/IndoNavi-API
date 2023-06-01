using IndoeNaviAPI.Models;
using IndoeNaviAPI.Models.Statistic;
using MongoDB.Bson;
using MongoDB.Driver;

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

    public async Task<List<PathSession>> GetPathSessions()
    {
        List<PathSession> pathSessions = await mongoDBService.GetAll<PathSession>("pathSessions");
        return pathSessions;
    }

    public async Task IncrementPathSessionToday()
    {
        string collectionName = "pathSessions";
        // Find path session for today
        PathSession pathsession = await mongoDBService.GetFirstByKey<PathSession, DateTimeOffset>(collectionName, "Date", DateTimeOffset.Now.Date);
        // If not exist then create one
        if (pathsession == null)
        {
            pathsession = new PathSession { Id= ObjectId.Empty, Date = DateTimeOffset.Now.Date, Count = 1 };
            await mongoDBService.Insert(pathsession, collectionName);
            return;
        }


        // Increment the counter field
        await mongoDBService.Update_IncrementField<PathSession>(collectionName, pathsession.Id, "Count", 1, pathsession);
        return;
    }

    public async Task<List<ActiveUser>> GetActiveUsers()
    {
        List<ActiveUser> activeUsers = await mongoDBService.GetAll<ActiveUser>("activeUsers");
        return activeUsers;
    }

    public async Task IncrementActiveUsersToday()
    {
        string collectionName = "activeUsers";
        // Find Active users for today
        ActiveUser activeUser = await mongoDBService.GetFirstByKey<ActiveUser, DateTimeOffset>(collectionName, "Date", DateTimeOffset.Now.Date);

        // If not exist then create one
        if (activeUser == null)
        {
            activeUser = new ActiveUser { Id = ObjectId.Empty, Date = DateTimeOffset.Now.Date, Count = 1 };
            await mongoDBService.Insert(activeUser, collectionName);
            return;
        }


        // Increment the counter field
        await mongoDBService.Update_IncrementField<ActiveUser>(collectionName, activeUser.Id, "Count", 1, activeUser);
        return;
    }

    public async Task<List<DestinationVisit>> GetDestinationVisits()
    {
        List<DestinationVisit> destinationVisits = await mongoDBService.GetAll<DestinationVisit>("destinationVisit");
        return destinationVisits;
    }

    public async Task IncrementDestinationVisits(string destination)
    {
        string collectionName = "destinationVisit";
        // Find Active users for today
        DestinationVisit destinationVisit = await mongoDBService.GetFirstByKey<DestinationVisit, string>(collectionName, "Destination", destination);

        // If not exist then create one
        if (destinationVisit == null)
        {
            destinationVisit = new DestinationVisit { Id = ObjectId.Empty, Destination = destination, Count = 1 };
            await mongoDBService.Insert(destinationVisit, collectionName);
            return;
        }


        // Increment the counter field
        await mongoDBService.Update_IncrementField<DestinationVisit>(collectionName, destinationVisit.Id, "Count", 1, destinationVisit);
        return;
    }

    public async Task<List<UsedSensor>> GetUsedSensors()
    {
        List<UsedSensor> usedSensors = await mongoDBService.GetAll<UsedSensor>("usedSensors");
        return usedSensors;
    }

    public async Task IncrementUsedSensors(string sensorName)
    {
        string collectionName = "usedSensors";
        // Find Active users for today
        UsedSensor usedSensor = await mongoDBService.GetFirstByKey<UsedSensor, string>(collectionName, "SensorName", sensorName);

        // If not exist then create one
        if (usedSensor == null)
        {
            usedSensor = new UsedSensor { Id = ObjectId.Empty, SensorName = sensorName, Count = 1 };
            await mongoDBService.Insert(usedSensor, collectionName);
            return;
        }


        // Increment the counter field
        await mongoDBService.Update_IncrementField<UsedSensor>(collectionName, usedSensor.Id, "Count", 1, usedSensor);
        return;
    }
}
