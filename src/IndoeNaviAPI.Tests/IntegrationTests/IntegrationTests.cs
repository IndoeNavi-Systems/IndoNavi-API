using IndoeNaviAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Text;

namespace IndoeNaviAPI.Tests.IntegrationTests;

public class IntegrationTests
{
	protected readonly HttpClient httpClient;

	public IntegrationTests()
	{
		WebApplicationFactory<Program> webApplicationFactory = new();
		httpClient = webApplicationFactory.CreateClient();
	}

	public HttpContent ConvertObjectToHttpContent(object obj)
	{
		return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
	}

	public async Task<T> GetObjectFromResponse<T>(HttpResponseMessage response)
	{
		return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
	}
}
