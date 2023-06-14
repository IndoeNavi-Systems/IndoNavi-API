using IndoeNaviAPI.Models;

namespace IndoeNaviAPI.Services;

public interface IStatisticService
{
	Task<List<T>> GetAllByArea<T>(string area);
    Task<bool> IsAreaExists(string area);
    Task IncrementStatisticsToday<T>(string area) where T : IAmDateValueStatistic, IHasAreaProp, new();
    Task IncrementNameListStatistic<T>(string area, string name) where T : IAmNameValueStatistic, IHasAreaProp, new();
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
        await mongoDBService.UpdateIncrementField<T>("Count", 1, obj);
    }

    /// <summary>
    /// This method will increment the count prop by one, for the specified name. It used for classes with Id, Area, Name and Count props and an empty constructer <br></br>
    /// If no documents exists for the name it will create one with Count 1
    /// </summary>
    /// <typeparam name="T">  T should implement <see cref="IAmNameValueStatistic"/>, <see cref="IHasAreaProp"/> and new() </typeparam>
    public async Task IncrementNameListStatistic<T>(string area, string name) where T : IAmNameValueStatistic, IHasAreaProp, new()
    {
        // Find object for today
        T obj = await mongoDBService.GetFirstByKey<T, string>("Name", name);
        // If not exist then create one
        if (obj == null)
        {
            obj = new T() { Name = name, Count = 1, Area = area };
            await mongoDBService.Insert<T>(obj);
            return;
        }

        // Increment the counter field
        await mongoDBService.UpdateIncrementField<T>("Count", 1, obj);
    }

    /// <summary>
    /// Check if area exits in map
    /// </summary>
    public async Task<bool> IsAreaExists(string area)
    {
        return (await mongoDBService.GetAllByKey<Map, string>("Area", area)).Count > 0;
    }
}
