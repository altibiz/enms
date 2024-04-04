using Enms.Fake;
using Enms.Fake.Client;
using Enms.Fake.Extensions;
using Enms.Fake.Generators.Abstractions;

var options = Options.Parse(args);

var serviceCollection = new ServiceCollection();

serviceCollection.AddLoaders();
serviceCollection.AddGenerators();
serviceCollection.AddClient();

#pragma warning disable ASP0000
var serviceProvider = serviceCollection.BuildServiceProvider();
#pragma warning restore ASP0000

while (true)
{
  var now = DateTimeOffset.UtcNow;
  var lastMinute = now.AddMinutes(-1);
  var pushClient = serviceProvider.GetRequiredService<EnmsPushClient>();
  var generators = serviceProvider.GetServices<IMeasurementGenerator>();

  var measurements = new List<string>();
  foreach (var meterId in options.MeterIds)
  {
    foreach (var generator in generators)
    {
      if (generator.CanGenerateMeasurementsFor(meterId))
      {
        measurements.AddRange(await generator
          .GenerateMeasurements(lastMinute, now, meterId));
      }
    }
  }

  foreach (var measurement in measurements)
  {
    await pushClient.Push(
      options.BaseUrl,
      options.MessengerId,
      options.ApiKey,
      measurement
    );
  }

  await Task.Delay(1000 * 60);
}
