using Enms.Email.Options;
using Enms.Email.Sender;
using Enms.Email.Sender.Abstractions;
using MailKit.Net.Smtp;

namespace Enms.Email.Extensions;

public static class IServiceCollectionExtensions
{
  public static IServiceCollection AddEnmsEmail(
    this IServiceCollection services,
    IHostApplicationBuilder builder
  )
  {
    services.Configure<EnmsEmailOptions>(
      builder.Configuration.GetSection("Enms:Email"));

    services.AddTransient<ISmtpClient, SmtpClient>();

    services.AddTransient<IEmailSender, SmtpSender>();

    return services;
  }
}
