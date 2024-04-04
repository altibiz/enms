using System.Globalization;
using CsvHelper;
using Enms.Fake.Loaders.Abstractions;

namespace Enms.Fake.Loaders;

public class CsvLoader<TRecord> : ILoader<List<TRecord>>
{
  public List<TRecord> Load(Stream stream)
  {
    using var streamReader = new StreamReader(stream);
    using var csvReader =
      new CsvReader(streamReader, CultureInfo.InvariantCulture);
    return csvReader.GetRecords<TRecord>().ToList();
  }
}
