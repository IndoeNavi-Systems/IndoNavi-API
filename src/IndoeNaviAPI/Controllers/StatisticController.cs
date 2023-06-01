using IndoeNaviAPI.Models;
using IndoeNaviAPI.Models.Statistic;
using IndoeNaviAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IndoeNaviAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class StatisticController : ControllerBase
{
	private readonly IStatisticService statisticService;

    public StatisticController(IStatisticService statisticService)
    {
        this.statisticService = statisticService;
    }

	[HttpGet("pathsessions")]
	public async Task<ActionResult<List<PathSession>>> GetPathSessions()
	{
		List<PathSession> pathSessions = await statisticService.GetPathSessions();
        if (pathSessions.Count <= 0)
		{
            return NotFound($"No pathSessions exists");
	}
        return pathSessions;
    }
	[HttpPost("pathsessions")]
	public async Task<IActionResult> IncrementPathSession()
	{
		await statisticService.IncrementPathSessionToday();
		return Ok($"Today's path session counter incremented with 1");
	}

	[HttpGet("activeusers")]
	public async Task<ActionResult<List<ActiveUsers>>> GetActiveUsers()
	{
        List<ActiveUsers> activeUsers = await statisticService.GetActiveUsers();
        if (activeUsers.Count <= 0)
		{
            return NotFound($"No activeUsers exists");
        }
        return activeUsers;
	}
	[HttpPost("activeusers")]
	public async Task<IActionResult> IncrementActiveUsers()
	{
        await statisticService.IncrementActiveUsersToday();
        return Ok($"Today's active user counter incremented with 1");
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
