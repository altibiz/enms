using Enms.Data.Timescale;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace Enms.Data.Extensions;

public static class DbContextOptionsBuilderExtensions
{
  public static DbContextOptionsBuilder UseTimescale(
    this DbContextOptionsBuilder builder,
    string connectionString,
    Action<NpgsqlDbContextOptionsBuilder>? npgsqlOptionsAction = null
  )
  {
    return builder
      .UseNpgsql(connectionString, npgsqlOptionsAction)
      .ReplaceService<IMigrationsSqlGenerator, TimescaleMigrationSqlGenerator>()
      .ReplaceService<IRelationalAnnotationProvider,
        TimescaleAnnotationProvider>();
  }
}
