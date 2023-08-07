using System.Text;

namespace FaceAutomation2
{
	public class Fetch
	{
		private static readonly HttpClient httpClient = new HttpClient();
		private static readonly string apiKey = "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";

		public static async Task<string> FetchData(string url, string requestBody)
		{
			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
			request.Headers.Add("Authorization", $"Bearer {apiKey}");
			request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

			HttpResponseMessage response = await httpClient.SendAsync(request);

			if (response.IsSuccessStatusCode)
			{
				string responseData = await response.Content.ReadAsStringAsync();
				return responseData;
			}

			throw new Exception($"Error: {response.StatusCode}");
		}


	}
}
