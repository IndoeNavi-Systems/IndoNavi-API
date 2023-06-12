using IndoeNaviAPI.Models;
using IndoeNaviAPI.Tests.Data;
using System.Net;

namespace IndoeNaviAPI.Tests.IntegrationTests;

[TestFixture]
public class MapControllerTests : IntegrationTests
{
	[Test]
	public async Task UpsertMap_WhenMapDataIsValid_Returns200Ok()
	{
		// Arrange
		Map map = MapData.Create();
		HttpResponseMessage mapGetResponse = await httpClient.GetAsync("./Map?area=test-map");
		if (mapGetResponse.StatusCode != HttpStatusCode.NotFound)
		{
			Map currentMap = await GetObjectFromResponse<Map>(mapGetResponse);
			map.Id = currentMap.Id;
		}

		// Act
		HttpResponseMessage mapUpsertResponse = await httpClient.PutAsync("./Map", ConvertObjectToHttpContent(map));

		// Assert
		Assert.That(mapUpsertResponse.StatusCode == HttpStatusCode.OK);
	}
}
