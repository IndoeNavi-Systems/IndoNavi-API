using IndoeNaviAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace IndoeNaviAPI.Services;

public interface IMongoDBService
{
	Task<List<T>> GetAllByKey<T, TFieldValue>(string collectionName, string filterKey, TFieldValue filterKeyValue);
	Task<T> GetFirstByKey<T, TFieldValue>(string collectionName, string filterKey, TFieldValue filterKeyValue);
	Task Insert<T>(T type, string collectionName);
	Task Upsert<T>(string collectionName, ObjectId filterKeyValue, T type) where T : IHasIdProp;
	Task<List<T>> GetAll<T>(string collectionName);
	Task Update_IncrementField<T>(string collectionName, ObjectId filterKeyValue, string fieldName, int incrementValue, T type) where T : IHasIdProp;

}

public class MongoDBService : IMongoDBService
{
	private readonly IMongoDatabase mongoDatabase;

	public MongoDBService(IMongoClient mongoClient)
	{
		mongoDatabase = mongoClient.GetDatabase("indoeNaviDB");
	}

	public Task Insert<T>(T type, string collectionName)
	{
		var collection = mongoDatabase.GetCollection<T>(collectionName);
		return collection.InsertOneAsync(type);
	}
	public Task Upsert<T>(string collectionName, ObjectId filterKeyValue, T type) where T : IHasIdProp
	{
		// Check if its a upsert or a update 
		if (filterKeyValue == ObjectId.Empty)
		{
			// If its a upsert then genereate new id to Map object
			filterKeyValue = ObjectId.GenerateNewId();
			type.Id = filterKeyValue;
		}
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
		return results.FirstOrDefault();
	}

	public Task Update_IncrementField<T>(string collectionName, ObjectId filterKeyValue, string fieldName, int incrementValue, T type) where T : IHasIdProp
	{
		// Check if its a upsert or a update 
		if (filterKeyValue == ObjectId.Empty)
		{
			// If its a upsert then genereate new id to Map object
			filterKeyValue = ObjectId.GenerateNewId();
			type.Id = filterKeyValue;
		}

		var collection = mongoDatabase.GetCollection<T>(collectionName);
		var filter = Builders<T>.Filter.Eq("_id", filterKeyValue);
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
