namespace Enms.Business.Models.Abstractions;

public interface IInvoice : IIdentifiable, IReadonly
{
  DateTimeOffset IssuedOn { get; }

  string? IssuedById { get; }

  DateTimeOffset FromDate { get; }

  DateTimeOffset ToDate { get; }
}
