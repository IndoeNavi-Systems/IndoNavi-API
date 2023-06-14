namespace IndoeNaviAPI.Utilities;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class MongoCollectionAttribute : Attribute
{
    public string Collection { get; }
    public MongoCollectionAttribute(string collection)
    {
        Collection = collection;
    }
}
