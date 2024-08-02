namespace Enms.Fake.Client;

// TODO: put some options into appsettings.json

public class EnmsPushClient(
  IHttpClientFactory httpClientFactory,
  ILogger<EnmsPushClient> logger
)
{
  public async Task Push(
    string meterId,
    string apiKey,
    HttpContent request,
    CancellationToken cancellationToken = default
  )
  {
    var client = httpClientFactory.CreateClient();
    client.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
    Console.WriteLine(client.BaseAddress);

    // FIXME: this should be configurable
#pragma warning disable S1075
    client.BaseAddress = new Uri("http://localhost:5000/");
#pragma warning restore S1075

    logger.LogInformation(
      "Pushing measurements to {BaseUrl} for meter {MeterId}",
      client.BaseAddress,
      meterId
    );

    var success = false;

    while (!success)
    {
      try
      {
        var response =
          await client.PostAsync(
            $"iot/push/{meterId}",
            request,
            cancellationToken
          );
        success = response.IsSuccessStatusCode;
        if (!success)
        {
          logger.LogWarning(
            "Failed to push measurements with {StatusCode}, retrying...",
            response.StatusCode
          );
        }

        await Task.Delay(1000, cancellationToken);
      }
      catch (Exception ex)
      {
        logger.LogError(ex, "Failed to push measurements");
      }
    }
  }
}
