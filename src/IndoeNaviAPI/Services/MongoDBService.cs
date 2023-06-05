using IndoeNaviAPI.Models;
using IndoeNaviAPI.Utilities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace IndoeNaviAPI.Services;

public interface IMongoDBService
{
	Task<List<T>> GetAllByKey<T, TFieldValue>(string filterKey, TFieldValue filterKeyValue);
	Task<T> GetFirstByKey<T, TFieldValue>(string filterKey, TFieldValue filterKeyValue);
	Task Insert<T>(T type);
	Task Upsert<T>(ObjectId filterKeyValue, T type) where T : IHasIdProp;
    Task<List<T>> GetAll<T>();
    Task Update_IncrementField<T>(ObjectId filterKeyValue, string fieldName, int incrementValue, T type) where T : IHasIdProp;
    void CreateUniqueAreaForMapIfNotExists<T>();
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
          .GetCollection<T>(Get<T>());
    }

    private static string Get<T>()
    {
        return (typeof(T).GetCustomAttributes(typeof(MongoCollectionAttribute), true).FirstOrDefault() as MongoCollectionAttribute)?.Collection;
    }

    /// <summary>
    /// Create a unique index for maps collection.
    /// The index is on Area ascending.
    /// </summary>
    public void CreateUniqueAreaForMapIfNotExists<T>()
    {
        var collectionExists = mongoDatabase.ListCollectionNames().ToList().Contains("maps");
        if (!collectionExists)
        {
            var collection = GetCollection<T>();
            var options = new CreateIndexOptions { Unique = true };
            collection.Indexes.CreateOne("{ Area : 1 }", options);
        }
    }

    public Task Insert<T>(T type)
    {
        var collection = GetCollection<T>();
        return collection.InsertOneAsync(type);
    }
    public Task Upsert<T>(ObjectId filterKeyValue, T type) where T : IHasIdProp
    {
        // Check if its a upsert or a update 
        if (filterKeyValue == ObjectId.Empty)
        {
            // If its a upsert then genereate new id to Map object
            filterKeyValue = ObjectId.GenerateNewId();
            type.Id = filterKeyValue;
        }
        var collection = GetCollection<T>();
        var filter = Builders<T>.Filter.Eq("_id", filterKeyValue);
        return collection.ReplaceOneAsync(filter, type, new ReplaceOptions { IsUpsert = true });
    }

    public async Task<List<T>> GetAllByKey<T, TFieldValue>(string filterKey, TFieldValue filterKeyValue)
    {
        var collection = GetCollection<T>();
        var filter = Builders<T>.Filter.Eq(filterKey, filterKeyValue);
        var results = await collection.FindAsync(filter);
        return results.ToList();
    }

    public async Task<T> GetFirstByKey<T, TFieldValue>(string filterKey, TFieldValue filterKeyValue)
    {
        var collection = GetCollection<T>();
        var filter = Builders<T>.Filter.Eq(filterKey, filterKeyValue);
        var results = await collection.FindAsync(filter);
        return results.FirstOrDefault();
    }

    public Task Update_IncrementField<T>(ObjectId filterKeyValue, string fieldName, int incrementValue, T type) where T : IHasIdProp
    {
        // Check if its a upsert or a update 
        if (filterKeyValue == ObjectId.Empty)
        {
            // If its a upsert then genereate new id to Map object
            filterKeyValue = ObjectId.GenerateNewId();
            type.Id = filterKeyValue;
        }

        var collection = GetCollection<T>();
        var filter = Builders<T>.Filter.Eq("_id", filterKeyValue);
        var updateDef = Builders<T>.Update.Inc(fieldName, incrementValue);
        return collection.UpdateOneAsync(filter, updateDef);
    }

    public async Task<List<T>> GetAll<T>()
    {
        var collection = GetCollection<T>();
        var filter = Builders<T>.Filter.Empty;
        var results = await collection.FindAsync(filter);
        return results.ToList();
    }
}
