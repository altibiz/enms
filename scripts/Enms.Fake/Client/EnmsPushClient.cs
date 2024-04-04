namespace Enms.Fake.Client;

public class EnmsPushClient
{
  private readonly IHttpClientFactory _httpClientFactory;

  public EnmsPushClient(IHttpClientFactory httpClientFactory)
  {
    _httpClientFactory = httpClientFactory;
  }

  public async Task Push(
    string baseUrl,
    string messengerId,
    string apiKey,
    string request
  )
  {
    var client = _httpClientFactory.CreateClient();
    client.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
    client.DefaultRequestHeaders.Add("Content-Type", "application/xml");

    var content = new StringContent(request);

    await client.PostAsync($"{baseUrl}/push/{messengerId}", content);
  }
}
