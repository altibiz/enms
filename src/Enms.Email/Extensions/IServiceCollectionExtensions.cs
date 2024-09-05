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
    // Options
    services.Configure<EnmsEmailOptions>(
      builder.Configuration.GetSection("Enms:Email"));

    // MailKit
    services.AddMailKit();

    // Sender
    services.AddTransient<IEmailSender, SmtpSender>();

    return services;
  }

  private static void AddMailKit(
    this IServiceCollection services
  )
  {
    services.AddTransient<ISmtpClient, SmtpClient>();
  }
}
