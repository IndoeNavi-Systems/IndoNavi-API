using IndoeNaviAPI.Models;
using IndoeNaviAPI.Models.Statistic;
using IndoeNaviAPI.Tests.Data;
using System.Net;

namespace IndoeNaviAPI.Tests.IntegrationTests;

public class StatisticControllerTests : IntegrationTests
{
	[Test]
	public async Task IncrementPathSessions_WhenDataIsValid_Returns200Ok()
	{
		// Act
		await httpClient.PostAsync("./Statistic/pathsessions?area=test-map", new StringContent(string.Empty));
		HttpResponseMessage beforeIncrementResponse = await httpClient.GetAsync("./Statistic/pathsessions?area=test-map");
		HttpResponseMessage incrementResponse = await httpClient.PostAsync("./Statistic/pathsessions?area=test-map", new StringContent(string.Empty));
		HttpResponseMessage afterIncrementResponse = await httpClient.GetAsync("./Statistic/pathsessions?area=test-map");

		List<PathSession> oldPathSessions = await GetObjectFromResponse<List<PathSession>>(beforeIncrementResponse);
		List<PathSession> newPathSessions = await GetObjectFromResponse<List<PathSession>>(afterIncrementResponse);

		// Assert
		Assert.Multiple(() =>
		{
			Assert.That(beforeIncrementResponse.StatusCode == HttpStatusCode.OK);
			Assert.That(incrementResponse.StatusCode == HttpStatusCode.OK);
			Assert.That(afterIncrementResponse.StatusCode == HttpStatusCode.OK);
			Assert.That(oldPathSessions.Last().Count + 1 == newPathSessions.Last().Count);
		});
	}
}
