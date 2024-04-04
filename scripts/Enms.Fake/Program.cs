using Enms.Fake;
using Enms.Fake.Extensions;
using Enms.Fake.Services;

var options = Options.Parse(args);
if (options is null)
{
  return;
}

var builder = Host.CreateApplicationBuilder();

_ = options switch
{
  PushOptions push => builder.Services
    .AddSingleton(push)
    .AddRecords()
    .AddLoaders()
    .AddGenerators()
    .AddClient(push.Timeout_s, push.BaseUrl)
    .AddHostedService<PushHostedService>(),
  SeedOptions seed => builder.Services
    .AddSingleton(seed)
    .AddRecords()
    .AddLoaders()
    .AddGenerators()
    .AddClient(seed.Timeout_s, seed.BaseUrl)
    .AddHostedService<SeedHostedService>(),
  _ => throw new InvalidOperationException($"Unknown options: {options}")
};

var app = builder.Build();

await app.RunAsync();
