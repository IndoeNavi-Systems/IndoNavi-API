using IndoeNaviAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace IndoeNaviAPI.Services;

public interface IMongoDBService
{
	Task<List<T>> GetAllByKey<T, TFieldValue>(string collectionName, string filterKey, TFieldValue filterKeyValue);
	Task<T> GetFirstByKey<T, TFieldValue>(string collectionName, string filterKey, TFieldValue filterKeyValue);
	Task Insert<T>(T type, string collectionName);
	Task Upsert<T>(string collectionName, T type) where T : IHasIdProp;
    Task<List<T>> GetAll<T>(string collectionName);
    Task Update_IncrementField<T>(string collectionName, string fieldName, int incrementValue, T type) where T : IHasIdProp;
    void SetUniqueKey<T>(string collectionName, IndexKeysDefinition<T> keysDefinition);
}

public class MongoDBService : IMongoDBService
{
    private readonly IMongoDatabase mongoDatabase;

    public MongoDBService(IMongoClient mongoClient)
    {
        mongoDatabase = mongoClient.GetDatabase("indoeNaviDB");
    }

    /// <summary>
    /// Create a unique index for maps collection.
    /// The index is on Area ascending.
    /// </summary>

    public void SetUniqueKey<T>(string collectionName, IndexKeysDefinition<T> keysDefinition)
    {
        var collectionExists = mongoDatabase.ListCollectionNames().ToList().Contains(collectionName);
        if (!collectionExists)
        {
            var collection = mongoDatabase.GetCollection<T>(collectionName);
            var options = new CreateIndexOptions { Unique = true };
            collection.Indexes.CreateOne(keysDefinition, options);
        }
    }

    public Task Insert<T>(T type, string collectionName)
    {
        var collection = mongoDatabase.GetCollection<T>(collectionName);
        return collection.InsertOneAsync(type);
    }
    public Task Upsert<T>(string collectionName, T type) where T : IHasIdProp
    {
        var collection = mongoDatabase.GetCollection<T>(collectionName);
        var filter = Builders<T>.Filter.Eq("_id", type.Id);
        return collection.ReplaceOneAsync(filter, type, new ReplaceOptions { IsUpsert = true});
    }

    public async Task<List<T>> GetAllByKey<T, TFieldValue>(string collectionName, string filterKey, TFieldValue filterKeyValue)
    {
        var collection = mongoDatabase.GetCollection<T>(collectionName);
        var filter = Builders<T>.Filter.Eq(filterKey, filterKeyValue);
        var results = await collection.FindAsync(filter);
        return results.ToList();
    }

    public async Task<T> GetFirstByKey<T, TFieldValue>(string collectionName, string filterKey, TFieldValue filterKeyValue)
    {
        var collection = mongoDatabase.GetCollection<T>(collectionName);
        var filter = Builders<T>.Filter.Eq(filterKey, filterKeyValue);
        var results = await collection.FindAsync(filter);
        return results.FirstOrDefault();
    }

    public Task Update_IncrementField<T>(string collectionName, string fieldName, int incrementValue, T type) where T : IHasIdProp
    {
        var collection = mongoDatabase.GetCollection<T>(collectionName);
        var filter = Builders<T>.Filter.Eq("_id", type.Id);
        var updateDef = Builders<T>.Update.Inc(fieldName, incrementValue);
        return collection.UpdateOneAsync(filter, updateDef);
    }

    public async Task<List<T>> GetAll<T>(string collectionName)
    {
        var collection = mongoDatabase.GetCollection<T>(collectionName);
        var filter = Builders<T>.Filter.Empty;
        var results = await collection.FindAsync(filter);
        return results.ToList();
    }
}
