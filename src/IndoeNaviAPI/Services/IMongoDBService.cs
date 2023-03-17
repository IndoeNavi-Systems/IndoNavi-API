using MongoDB.Bson;

namespace IndoeNaviAPI.Services
{
    public interface IMongoDBService
    {
        Task<List<T>> GetAllByKey<T, TFieldValue>(string collectionName, string filterKey, TFieldValue filterKeyValue);
        Task<T> GetFirstByKey<T, TFieldValue>(string collectionName, string filterKey, TFieldValue filterKeyValue);
        Task Insert<T>(T type, string collectionName);
        Task Update<T>(T type, string collectionName, ObjectId filterKeyValue);
    }
}