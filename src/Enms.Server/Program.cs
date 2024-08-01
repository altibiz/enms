using Enms.Business.Extensions;
using Enms.Client.Extensions;
using OrchardCore.Logging;

var builder = WebApplication.CreateBuilder(args);
if (builder.Environment.IsDevelopment())
{
  builder.Host.UseNLogHost();
}

builder.Services
  .AddOrchardCms()
  .AddSetupFeatures("OrchardCore.AutoSetup")
  .ConfigureServices(
    services => services
      .AddEnmsClient(builder)
      .AddEnmsBusinessClient(builder))
  .Configure(
    (_, endpoints) => endpoints
      .MapEnmsClient("App", "Index", "/app")
      .MapEnmsIot("Iot", "Push", "/iot/push"))
  .Configure(
    app => app
      .MigrateEnmsData());

var app = builder.Build();
app.UseStaticFiles();
app.UseOrchardCore();
await app.RunAsync();
