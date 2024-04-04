using Enms.Business.Extensions;
using Enms.Client.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services
  .AddOrchardCms()
  .AddSetupFeatures("OrchardCore.AutoSetup")
  .ConfigureServices(services => services
    .AddEnmsClient(builder.Environment.IsDevelopment())
    .AddEnmsBusinessClient())
  .Configure((_, endpoints) => endpoints
    .MapEnmsClient("App", "Index", "/app"))
  .Configure(app => app
    .MigrateEnmsData());

var app = builder.Build();
app.UseStaticFiles();
app.UseOrchardCore();
await app.RunAsync();
