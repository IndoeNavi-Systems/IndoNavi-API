using IndoeNaviAPI.Models;
using IndoeNaviAPI.Models.Statistic;

namespace IndoeNaviAPI.Services;

public interface IStatisticService
{
	Task<List<T>> GetAllByArea<T>(string area);
    Task IncrementDestinationVisits(string destination, string area);    
    Task IncrementUsedSensors(string sensorName, string area);
    Task<bool> IsAreaExists(string area);
    Task IncrementStatisticsToday<T>(string area) where T : IAmDateValueStatistic, IHasAreaProp, new();

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

    /// <summary>
    /// This method will increment the count prop by one, for date equal Today. It used for classes with Id, Area, Date, Count props and an empty constructer <br></br>
    /// If no documents exists for today it will create one with Count 1
    /// </summary>
    /// <typeparam name="T"> T should implement <see cref="IAmDateValueStatistic"/>, <see cref="IHasAreaProp"/> and new() </typeparam>
    /// <param name="area"></param>
    /// <returns></returns>
    public async Task IncrementStatisticsToday<T>(string area) where T : IAmDateValueStatistic, IHasAreaProp, new()
    {
        // Find object for today
        T obj = await mongoDBService.GetFirstByKey<T, DateTime>("Date", DateTime.UtcNow.Date);
        // If not exist then create one
        if (obj == null)
        {
            obj = new T() { Date = DateTime.UtcNow.Date, Count = 1, Area = area };
            await mongoDBService.Insert<T>(obj);
            return;
        }

        // Increment the counter field
        await mongoDBService.Update_IncrementField<T>("Count", 1, obj);
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
