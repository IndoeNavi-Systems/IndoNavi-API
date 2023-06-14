using IndoeNaviAPI.Models;
using IndoeNaviAPI.Utilities;
using MongoDB.Driver;

namespace IndoeNaviAPI.Services;

public interface IMongoDBService
{
	Task<List<T>> GetAllByKey<T, TFieldValue>(string filterKey, TFieldValue filterKeyValue);
	Task<T> GetFirstByKey<T, TFieldValue>(string filterKey, TFieldValue filterKeyValue);
	Task Insert<T>(T type) where T : IHasIdProp; 
	Task Upsert<T>(T type) where T : IHasIdProp;
    Task<List<T>> GetAll<T>();
    Task UpdateIncrementField<T>(string fieldName, int incrementValue, T type) where T : IHasIdProp;
    void SetUniqueKey<T>(string collectionName, IndexKeysDefinition<T> keysDefinition);
}

public class MongoDBService : IMongoDBService
{
    private readonly IMongoDatabase mongoDatabase;

    public MongoDBService(IMongoClient mongoClient)
    {
        mongoDatabase = mongoClient.GetDatabase("indoeNaviDB"); 
    }

    private IMongoCollection<T> GetCollection<T>(ReadPreference readPreference = null)
    {
        return mongoDatabase
          .WithReadPreference(readPreference ?? ReadPreference.Primary)
          .GetCollection<T>(GetCollectionName<T>());
    }

    private static string? GetCollectionName<T>()
    {
        return (typeof(T).GetCustomAttributes(typeof(MongoCollectionAttribute), true).FirstOrDefault() as MongoCollectionAttribute)?.Collection;
    }

    /// <summary>
    /// Create a unique index for maps collection.
    /// The index is on Area ascending.
    /// </summary>

    public void SetUniqueKey<T>(string collectionName, IndexKeysDefinition<T> keysDefinition)
    {
		bool collectionExists = mongoDatabase.ListCollectionNames().ToList().Contains(collectionName);
        if (!collectionExists)
        {
			IMongoCollection<T> collection = GetCollection<T>();
			CreateIndexOptions options = new() { Unique = true };
            collection.Indexes.CreateOne(keysDefinition, options);
        }
    }

    public Task Insert<T>(T type) where T : IHasIdProp
    {
        if (type.Id == Guid.Empty)
        {
            type.Id = Guid.NewGuid();
        }
		IMongoCollection<T> collection = GetCollection<T>();
        return collection.InsertOneAsync(type);
    }
    public async Task Upsert<T>(T type) where T : IHasIdProp
    {
        if (type.Id == Guid.Empty)
        {
            type.Id = Guid.NewGuid();
        }
		IMongoCollection<T> collection = GetCollection<T>();
		FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", type.Id);
        await collection.ReplaceOneAsync(filter, type, new ReplaceOptions { IsUpsert = true});
    }

    public async Task<List<T>> GetAllByKey<T, TFieldValue>(string filterKey, TFieldValue filterKeyValue)
    {
		IMongoCollection<T> collection = GetCollection<T>();
		FilterDefinition<T> filter = Builders<T>.Filter.Eq(filterKey, filterKeyValue);
		IAsyncCursor<T> results = await collection.FindAsync(filter);
        return results.ToList();
    }

    public async Task<T> GetFirstByKey<T, TFieldValue>(string filterKey, TFieldValue filterKeyValue)
    {
		IMongoCollection<T> collection = GetCollection<T>();
		FilterDefinition<T> filter = Builders<T>.Filter.Eq(filterKey, filterKeyValue);
		IAsyncCursor<T> results = await collection.FindAsync(filter);
        return results.FirstOrDefault();
    }

    public async Task UpdateIncrementField<T>(string fieldName, int incrementValue, T type) where T : IHasIdProp
    {
		IMongoCollection<T> collection = GetCollection<T>();
		FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", type.Id);
		UpdateDefinition<T> updateDef = Builders<T>.Update.Inc(fieldName, incrementValue);
        await collection.UpdateOneAsync(filter, updateDef);
    }

    public async Task<List<T>> GetAll<T>()
    {
		IMongoCollection<T> collection = GetCollection<T>();
		FilterDefinition<T> filter = Builders<T>.Filter.Empty;
		IAsyncCursor<T> results = await collection.FindAsync(filter);
        return results.ToList();
    }
}
