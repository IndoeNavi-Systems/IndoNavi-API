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
	public async Task<ActionResult<List<ActiveUser>>> GetActiveUsers()
	{
        List<ActiveUser> activeUsers = await statisticService.GetActiveUsers();
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
	public async Task<ActionResult<List<DestinationVisit>>> GetDestinationVisits()
	{
        List<DestinationVisit> destinationVisits = await statisticService.GetDestinationVisits();
        if (destinationVisits.Count <= 0)
        {
            return NotFound($"No destination Visits exists");
        }
        return destinationVisits;
    }
	[HttpPost("destinationvisits")]
	public async Task<IActionResult> IncrementDestinationVisit(string destination)
	{
        await statisticService.IncrementDestinationVisits(destination);
        return Ok($"Destination visit {destination} incremented with 1");
	}

    [HttpGet("usedsensor")]
    public async Task<ActionResult<List<UsedSensor>>> GetUsedSensors()
    {
        List<UsedSensor> usedSensors = await statisticService.GetUsedSensors();
        if (usedSensors.Count <= 0)
        {
            return NotFound($"No used sensors exists");
        }
        return usedSensors;
    }
    [HttpPost("usedsensor")]
    public async Task<IActionResult> IncrementUsedSensor(string sensorName)
    {
        await statisticService.IncrementUsedSensors(sensorName);
        return Ok($"Sensor {sensorName} incremented used with 1");
    }
}
