namespace Enms.Email.Options;

public record EnmsEmailFromOptions(
  string Name,
  string Address
);

public record EnmsEmailSmtpOptions(
  string Host,
  int Port,
  string Username,
  string Password,
  bool Ssl
);

public record EnmsEmailOptions(
  EnmsEmailSmtpOptions Smtp,
  EnmsEmailFromOptions From
);
