using IndoeNaviAPI.Models.Statistic;
using Microsoft.AspNetCore.Mvc;

namespace IndoeNaviAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class StatisticController : ControllerBase
{
	[HttpGet("path/sessions")]
	public List<DateValue> GetPathSessions(string area)
	{
		return new List<DateValue>()
		{
			new DateValue { DateOnly = new DateOnly(2023, 3, 17), Value = 11 },
			new DateValue { DateOnly = new DateOnly(2023, 3, 16), Value = 35 },
			new DateValue { DateOnly = new DateOnly(2023, 3, 15), Value = 23 },
			new DateValue { DateOnly = new DateOnly(2023, 3, 14), Value = 13 },
			new DateValue { DateOnly = new DateOnly(2023, 3, 13), Value = 25 },
		};
	}
	[HttpPost("pathsessions")]
	public IActionResult IncrementPathSession(string area)
	{
		return Ok($"Today's path session counter incremented with 1 {area}");
	}

	[HttpGet("activeusers")]
	public List<DateValue> GetActiveUsers(string area)
	{
		return new List<DateValue>()
		{
			new DateValue { DateOnly = new DateOnly(2023, 3, 17), Value = 64 },
			new DateValue { DateOnly = new DateOnly(2023, 3, 16), Value = 54 },
			new DateValue { DateOnly = new DateOnly(2023, 3, 15), Value = 25 },
			new DateValue { DateOnly = new DateOnly(2023, 3, 14), Value = 65 },
			new DateValue { DateOnly = new DateOnly(2023, 3, 13), Value = 23 },
		};
	}
	[HttpPost("activeusers")]
	public IActionResult IncrementActiveUsers(string area)
	{
		return Ok($"Today's active user counter incremented with 1 {area}");
	}

	[HttpGet("destinationvisits")]
	public List<DestinationVisit> GetDestinationVisits(string area)
	{
		return new List<DestinationVisit>()
		{
			new DestinationVisit { Destination = "D30", VisitAmount = 43 },
			new DestinationVisit { Destination = "D31", VisitAmount = 23 },
			new DestinationVisit { Destination = "D32", VisitAmount = 64 },
		};
	}
	[HttpPost("destinationvisits")]
	public IActionResult IncrementDestinationVisit(string area, string destination)
	{
		return Ok($"Destination visit {destination} incremented with 1 on {area}");
	}
}
