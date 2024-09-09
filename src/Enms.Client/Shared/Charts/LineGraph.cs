using ApexCharts;
using Enms.Business.Aggregation.Agnostic;
using Enms.Business.Models;
using Enms.Business.Models.Abstractions;
using Enms.Business.Models.Enums;
using Enms.Business.Observers.Abstractions;
using Enms.Business.Observers.EventArgs;
using Enms.Business.Queries;
using Enms.Business.Queries.Abstractions;
using Enms.Client.Base;
using Enms.Client.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;

// FIXME: chart updates are always behind by one render

namespace Enms.Client.Shared.Charts;

#pragma warning disable S3881 // "IDisposable" should be implemented correctly
public partial class LineGraph : EnmsOwningComponentBase
#pragma warning restore S3881 // "IDisposable" should be implemented correctly
{
  [Parameter]
  public List<ILine> Lines { get; set; } = default!;

  [Parameter]
  public DateTimeOffset Timestamp { get; set; } = default!;

  [Parameter]
  public MeasureModel Measure { get; set; } = MeasureModel.ActivePower;

  [Parameter]
  public ResolutionModel Resolution { get; set; } = ResolutionModel.Minute;

  [Parameter]
  public HashSet<PhaseModel> Phases { get; set; } =
    Enum.GetValues<PhaseModel>().ToHashSet();

  [Parameter]
  public int Multiplier { get; set; } = 5;

  [Parameter]
  public bool Refresh { get; set; } = true;

  [CascadingParameter]
  public Breakpoint Breakpoint { get; set; } = default!;

  [Inject]
  public IMeasurementSubscriber MeasurementSubscriber { get; set; } = default!;

  [Inject]
  public AgnosticAggregateUpserter AggregateUpserter { get; set; } = default!;

  private ApexChart<IMeasurement>? _chart = default!;

  private PaginatedList<IMeasurement> _measurements = new(
    new List<IMeasurement>(), 0);

  private ApexChartOptions<IMeasurement> _options =
    new ApexChartOptions<IMeasurement>().WithFixedScriptPath();

  private HashSet<ILine> _selectedLines = new();

  protected override void OnInitialized()
  {
    MeasurementSubscriber.SubscribeUpsert(OnUpsert);

    _options = CreateGraphOptions();

    _selectedLines = Lines.Take(1).ToHashSet();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing)
    {
      MeasurementSubscriber.UnsubscribeUpsert(OnUpsert);
    }
  }

  protected override async Task OnInitializedAsync()
  {
    await OnParametersSetAsync();
  }

  protected override async Task OnParametersSetAsync()
  {
    var queries = ScopedServices.GetRequiredService<LineGraphQueries>();

    _measurements = await queries.Read(
      Lines,
      Resolution,
      Multiplier,
      fromDate: Timestamp
    );
    _options = CreateGraphOptions();

    if (_chart is { } chart)
    {
      await chart.UpdateSeriesAsync(animate: true);
      await chart.UpdateOptionsAsync(false, true, false);
    }
  }

  private void OnUpsert(
    object? _sender,
    MeasurementUpsertEventArgs args)
  {
    if (!Refresh)
    {
      return;
    }

    Task.Run(async () =>
    {
      var now = DateTimeOffset.UtcNow;
      var timestamp = _measurements.Items.LastOrDefault()?.Timestamp ?? now;
      var timeSpan = Resolution.ToTimeSpan(Multiplier, timestamp);
      var appropriateInterval = QueryConstants.AppropriateInterval(timeSpan, now);

      if (appropriateInterval is null)
      {
        var newMeasurements = args.Measurements
          .Where(x => x.Timestamp >= timestamp)
          .Where(x => Lines.Exists(line =>
            line.Id == x.LineId
            && line.MeterId == x.MeterId))
          .OrderBy(x => x.Timestamp)
          .ToList();
        var concatenated = _measurements.Items.Concat(newMeasurements).ToList();
        _measurements = new PaginatedList<IMeasurement>(
          concatenated,
          _measurements.TotalCount + newMeasurements.Count
        );

        if (_chart is { } chart)
        {
          await chart.AppendDataAsync(newMeasurements);
        }
      }
      else
      {
        var newAggregates = args.Aggregates
          .Where(x => x.Timestamp >= timestamp)
          .Where(x => x.Interval == appropriateInterval)
          .Where(x => Lines.Exists(line =>
            line.Id == x.LineId
            && line.MeterId == x.MeterId))
          .OrderBy(x => x.Timestamp)
          .OfType<IAggregate>()
          .ToList();
        var aggregated = _measurements.Items.OfType<IAggregate>()
          .Concat(newAggregates)
          .GroupBy(x => x.Timestamp)
          .Select(x => x
            .Aggregate(AggregateUpserter.UpsertModelAgnostic))
          .OfType<IMeasurement>()
          .ToList();
        _measurements = new PaginatedList<IMeasurement>(
          aggregated.ToList(),
          _measurements.TotalCount - _measurements.Items.Count + aggregated.Count
        );

        if (_chart is { } chart)
        {
          await chart.UpdateSeriesAsync();
        }
      }
    });
  }

  private ApexChartOptions<IMeasurement> CreateGraphOptions()
  {
    var maxPower = _measurements.Items
      .Select(x => x.ActivePower_W.TariffUnary().DuplexImport().PhaseSum())
      .OrderByDescending(x => x)
      .Cast<decimal?>()
      .FirstOrDefault();

    var options = _options;
    foreach (var line in _selectedLines)
    {
      options = _options.WithActivePower(
        $"{line.Id} {Translate("CONNECTION POWER")}",
        line.ConnectionPower_W,
        maxPower
      );
    }

    var measure = $"{Translate(Measure.ToTitle())} ({Measure.ToUnit()})";
    options = Breakpoint <= Breakpoint.Sm
      ? options.WithSmAndDown(measure)
      : options.WithMdAndUp(measure);

    return options;
  }
}
