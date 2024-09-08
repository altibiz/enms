namespace Enms.Email.Options;

public class EnmsEmailFromOptions
{
  public string Name { get; set; } = default!;
  public string Address { get; set; } = default!;
}

public class EnmsEmailSmtpOptions
{
  public string Host { get; set; } = default!;
  public int Port { get; set; }
  public string Username { get; set; } = default!;
  public string Password { get; set; } = default!;
  public bool Ssl { get; set; }
}

public class EnmsEmailOptions
{
  public EnmsEmailSmtpOptions Smtp { get; set; } = default!;

  public EnmsEmailFromOptions From { get; set; } = default!;
}
