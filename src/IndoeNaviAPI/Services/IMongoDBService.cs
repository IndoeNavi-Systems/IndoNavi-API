using IndoeNaviAPI.Models;
using MongoDB.Bson;

namespace IndoeNaviAPI.Services
{
    public interface IMongoDBService
    {
        Task<List<T>> GetAllByKey<T, TFieldValue>(string collectionName, string filterKey, TFieldValue filterKeyValue);
        Task<T> GetFirstByKey<T, TFieldValue>(string collectionName, string filterKey, TFieldValue filterKeyValue);
        Task Insert<T>(T type, string collectionName);
        Task Upsert<T>(string collectionName, ObjectId filterKeyValue, T type) where T : IHasIdProp;
    }
}