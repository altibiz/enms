using Enms.Business.Extensions;
using Enms.Client.Extensions;
using Enms.Data.Extensions;
using Enms.Email.Extensions;
using Enms.Server.Extensions;
using Enms.Users.Extensions;
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
      .AddEnmsUsers(builder)
      .AddEnmsData(builder)
      .AddEnmsEmail(builder)
      .AddEnmsBusiness(builder)
      .AddEnmsClient(builder)
      .AddEnmsServer(builder))
  .Configure(
    (app, endpoints) => app
      .UseEnmsServer(endpoints)
      .UseEnmsClient(endpoints)
      .UseEnmsBusiness(endpoints)
      .UseEnmsData(endpoints)
      .UseEnmsEmail(endpoints)
      .UseEnmsUsers(endpoints));

var app = builder.Build();

app.UseStaticFiles();
app.UseOrchardCore();

await app.RunAsync();
