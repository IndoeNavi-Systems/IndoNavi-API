using IndoeNaviAPI.Models;
using IndoeNaviAPI.Services;
using Moq;

namespace IndoeNaviAPI.Tests
{
	[TestFixture]
	public class MapServiceTest
	{
		[Test]
		public void UpsertMap_IsAreaNotUnique_TrowDuplicate()
		{
			// Arrange
			Map input = new Map() { ImageData = "asds" }; // asds is not a valid base64 string

			Mock<IMongoDBService> mockMongoDB = new Mock<IMongoDBService>();
			mockMongoDB.Setup(m => m.Upsert<Map>(input)).ThrowsAsync(new Exception("duplicate"));
			MapService mapService = new MapService(mockMongoDB.Object);

			// Act and Assert
			var ex = Assert.ThrowsAsync<Exception>(async () => { await mapService.UpsertMap(input); });
			mockMongoDB.Verify(m => m.Upsert(input), Times.AtLeastOnce);
			StringAssert.Contains("duplicate", ex.Message);
		}

		[Test]
		public void UpsertMap_IsImageStringBase64_TrowFormatException()
		{
			// Arrange
			Map input = new Map() { ImageData = "asd" }; // asd is not a valid base64 string

			Mock<IMongoDBService> mockMongoDB = new Mock<IMongoDBService>();
			MapService mapService = new MapService(mockMongoDB.Object);

			// Act and Assert
			var ex = Assert.ThrowsAsync<FormatException>(async () => { await mapService.UpsertMap(input); });

			StringAssert.Contains("Image is not encoded in base64", ex.Message);
		}


		[TestCaseSource("GetMockMapList")]
		public async Task GetMap_IsDestinationExist_ReturnMapObject(Map input)
		{
			// Arrange
			Mock<IMongoDBService> mockMongoDB = new Mock<IMongoDBService>();
			mockMongoDB.Setup(m => m.GetFirstByKey<Map, string>("Area", input.Area)).ReturnsAsync(() =>
			{
				return input;
			});
			MapService mapService = new MapService(mockMongoDB.Object);

			// Act
			var result = await mapService.GetMap(input.Area);

			// Assert
			Assert.AreEqual(input.Area, result.Area);

		}

		private static List<Map> GetMockMapList()
		{
			List<Map> mock = new List<Map>
			{
				new Map() { Area = "Test1" },
				new Map() { Area = "Test2" },
				new Map() { Area = "Test3" },
				new Map() { Area = "Test4" },
				new Map() { Area = "Test5" }
			};
			return mock;
		}
	}
}
