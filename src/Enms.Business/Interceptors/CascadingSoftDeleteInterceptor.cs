using System.Security.Claims;
using Enms.Business.Extensions;
using Enms.Data;
using Enms.Data.Entities;
using Enms.Data.Entities.Base;
using Enms.Data.Entities.Enums;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

// TODO: implement cascading soft delete

namespace Enms.Business.Interceptors;

public class CascadingSoftDeleteInterceptor : ServedSaveChangesInterceptor
{
  public CascadingSoftDeleteInterceptor(IServiceProvider serviceProvider)
    : base(serviceProvider)
  {
  }

  public override int Order
  {
    get { return 11; }
  }

  public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
    DbContextEventData eventData,
    InterceptionResult<int> result,
    CancellationToken cancellationToken = default)
  {
    await AddCascadingSoftDeletes(eventData);
    return await base.SavingChangesAsync(eventData, result, cancellationToken);
  }

  private async ValueTask AddCascadingSoftDeletes(
    DbContextEventData eventData)
  {
    var context = eventData.Context as EnmsDataDbContext;
    if (context is null)
    {
      return;
    }

    var representativeId = await GetRepresentativeId(eventData);
    var now = DateTimeOffset.UtcNow;

    var entries = context.ChangeTracker
      .Entries<AuditableEntity>()
      .ToList();

    foreach (var auditable in entries)
    {
      if (auditable.State is EntityState.Modified &&
        auditable.Entity.IsDeleted &&
        !auditable.OriginalValues
          .GetValue<bool>(nameof(AuditableEntity.IsDeleted)))
      {
        var relationships = context.Model
          .GetEntityTypes()
          .SelectMany(e => e.GetForeignKeys())
          .Where(relationship => relationship.IsRequired)
          .Where(
            relationship => relationship.PrincipalEntityType
              == auditable.Metadata);

        foreach (var relationship in relationships)
        {
          if (!relationship.DeclaringEntityType.ClrType
            .IsAssignableTo(typeof(AuditableEntity)))
          {
            continue;
          }

          var declarers = await context
            .GetQueryable(relationship.DeclaringEntityType.ClrType)
            .Where(
              context.ForeignKeyEqualsAgnostic(
                relationship.PrincipalEntityType.ClrType,
                relationship.GetNavigation(true)?.Name
                ?? throw new InvalidOperationException(
                  "No navigation property found"),
                auditable.Entity.Id))
            .ToListAsync();

          foreach (var declaring in declarers)
          {
            SoftDelete(
              eventData,
              now,
              representativeId,
              context.Entry((AuditableEntity)declaring)
            );
          }
        }
      }
    }
  }

  private void SoftDelete(
    DbContextEventData eventData,
    DateTimeOffset now,
    string? representativeId,
    EntityEntry<AuditableEntity> auditable)
  {
    if (eventData.Context is null)
    {
      return;
    }

    auditable.State = EntityState.Modified;
    auditable.Entity.IsDeleted = true;
    auditable.Entity.DeletedOn = now;
    if (representativeId is not null)
    {
      auditable.Entity.DeletedById = representativeId;
      eventData.Context.Add(
        new RepresentativeAuditEventEntity
        {
          Timestamp = now,
          Title =
            $"Deleted {
              auditable.Entity.GetType().Name
            } {
              auditable.Entity.Title
            }",
          RepresentativeId = representativeId,
          Level = LevelEntity.Debug,
          Audit = AuditEntity.Deletion,
          Description = CreateDeletedMessage(auditable),
          AuditableEntityId = auditable.Entity.Id,
          AuditableEntityType = auditable.Entity.GetType().FullName
            ?? throw new InvalidOperationException(
              "No type name found for auditable entity"),
          AuditableEntityTable = auditable.Metadata.GetTableName()
            ?? throw new InvalidOperationException(
              "No table name found for auditable entity")
        });
    }
    else
    {
      auditable.Entity.DeletedById = null;
      eventData.Context.Add(
        new SystemAuditEventEntity
        {
          Timestamp = now,
          Title =
            $"Deleted {
              auditable.Entity.GetType().Name
            } {
              auditable.Entity.Title
            }",
          Level = LevelEntity.Debug,
          Audit = AuditEntity.Deletion,
          Description = CreateDeletedMessage(auditable),
          AuditableEntityId = auditable.Entity.Id,
          AuditableEntityType = auditable.Entity.GetType().FullName
            ?? throw new InvalidOperationException(
              "No type name found for auditable entity"),
          AuditableEntityTable = auditable.Metadata.GetTableName()
            ?? throw new InvalidOperationException(
              "No table name found for auditable entity")
        });
    }
  }

  private async Task<string?> GetRepresentativeId(DbContextEventData eventData)
  {
    ClaimsPrincipal? claimsPrincipal = null;
    if (serviceProvider
        .GetService<IHttpContextAccessor>()
        ?.HttpContext is { } httpContext)
    {
      claimsPrincipal = httpContext.User;
    }

    if (claimsPrincipal is null
      && serviceProvider.GetService<AuthenticationStateProvider>()
        is { } authStateProvider)
    {
      claimsPrincipal =
        (await authStateProvider.GetAuthenticationStateAsync()).User;
    }

    if (claimsPrincipal is null)
    {
      return null;
    }

    var id = claimsPrincipal.Claims
      .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
    if (string.IsNullOrEmpty(id))
    {
      return null;
    }

    var context = eventData.Context as EnmsDataDbContext;
    if (context is null)
    {
      return null;
    }

    var representative = await context.Representatives
      .Where(context.PrimaryKeyEquals<RepresentativeEntity>(id))
      .FirstOrDefaultAsync();

    return representative?.Id;
  }

  private static string CreateDeletedMessage(EntityEntry entry)
  {
    return entry.Properties
      .Where(property => property.Metadata.IsPrimaryKey())
      .Aggregate(
        $"Deleting {entry.Metadata.DisplayName()} with ",
        (auditString, property) => auditString +
          $"{property.Metadata.Name}: '{property.CurrentValue}' ");
  }
}
