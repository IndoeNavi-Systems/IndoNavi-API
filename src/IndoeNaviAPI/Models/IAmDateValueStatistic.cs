namespace IndoeNaviAPI.Models
{
    public interface IAmDateValueStatistic : IHasIdProp
    {
        DateTime Date { get; set; }
        
        //The value field
        int Count { get; set; }
    }
}
