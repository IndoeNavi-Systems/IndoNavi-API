using MongoDB.Bson;
using MongoDB.Driver;

namespace IndoeNaviAPI.Services;

public class MongoDBService : IMongoDBService
{
    private IMongoDatabase mongoDatabase;

    public MongoDBService(IMongoClient mongoClient)
    {
        this.mongoDatabase = mongoClient.GetDatabase("indoeNaviDB");
    }

    public Task Insert<T>(T type, string collectionName)
    {
        var collection = mongoDatabase.GetCollection<T>(collectionName);
        return collection.InsertOneAsync(type);
    }

    public Task Update<T>(T type, string collectionName, ObjectId filterKeyValue)
    {
        var collection = mongoDatabase.GetCollection<T>(collectionName);
        var filter = Builders<T>.Filter.Eq("_id", filterKeyValue);
        return collection.ReplaceOneAsync(filter, type, new ReplaceOptions { IsUpsert = true });

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
        return results.First();
    }
}
