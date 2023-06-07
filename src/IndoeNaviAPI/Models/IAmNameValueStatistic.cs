namespace IndoeNaviAPI.Models
{
    public interface IAmNameValueStatistic : IHasIdProp
    {
        string Name { get; set; }
        //The value field
        int Count { get; set; }
    }
}
