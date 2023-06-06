﻿using IndoeNaviAPI.Models.Statistic;
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
	public async Task<ActionResult<List<PathSession>>> GetPathSessions(string area)
	{
		List<PathSession> pathSessions = await statisticService.GetAllByArea<PathSession>(area);
		if (!pathSessions.Any())
        {
            return NotFound($"No pathSessions exists in this area {area}");
        }
        return pathSessions;
    }
	[HttpPost("pathsessions")]
	public async Task<IActionResult> IncrementPathSession(string area)
	{
        await statisticService.IncrementPathSessionToday();
        await statisticService.IncrementPathSessionToday(area);
		return Ok($"Today's path session counter incremented with 1");
	}

	[HttpGet("activeusers")]
	public async Task<ActionResult<List<ActiveUser>>> GetActiveUsers(string area)
	{
        List<ActiveUser> activeUsers = await statisticService.GetAllByArea<ActiveUser>(area);
        if (!activeUsers.Any())
        {
            return NotFound($"No activeUsers exists in this area {area}");
        }
        return activeUsers;
    }
	[HttpPost("activeusers")]
	public async Task<IActionResult> IncrementActiveUsers(string area)
	{
        await statisticService.IncrementActiveUsersToday();
        await statisticService.IncrementActiveUsersToday(area);
        return Ok($"Today's active user counter incremented with 1");
	}

	[HttpGet("destinationvisits")]
	public async Task<ActionResult<List<DestinationVisit>>> GetDestinationVisits(string area)
	{
        List<DestinationVisit> destinationVisits = await statisticService.GetAllByArea<DestinationVisit>(area);
        if (!destinationVisits.Any())
        {
            return NotFound($"No destination Visits exists in this area {area}");
        }
        return destinationVisits;
    }
	[HttpPost("destinationvisits")]
	public async Task<IActionResult> IncrementDestinationVisit(string destination, string area)
	{
        await statisticService.IncrementDestinationVisits(destination);
        await statisticService.IncrementDestinationVisits(destination, area);
        return Ok($"Destination visit {destination} incremented with 1");
	}

    [HttpGet("usedsensor")]
    public async Task<ActionResult<List<UsedSensor>>> GetUsedSensors(string area)
    {
        List<UsedSensor> usedSensors = await statisticService.GetAllByArea<UsedSensor>(area);
        if (!usedSensors.Any())
        {
            return NotFound($"No used sensors exists in this area {area}");
        }
        return usedSensors;
    }
    [HttpPost("usedsensor")]
    public async Task<IActionResult> IncrementUsedSensor(string sensorName, string area)
    {
        await statisticService.IncrementUsedSensors(sensorName);
        await statisticService.IncrementUsedSensors(sensorName, area);
        return Ok($"Sensor {sensorName} incremented used with 1");
    }
}
