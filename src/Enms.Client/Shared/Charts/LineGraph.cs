using ApexCharts;
using Enms.Business.Aggregation.Agnostic;
using Enms.Business.Models;
using Enms.Business.Models.Abstractions;
using Enms.Business.Models.Enums;
using Enms.Business.Observers.Abstractions;
using Enms.Business.Observers.EventArgs;
using Enms.Business.Queries.Abstractions;
using Enms.Business.Queries.Agnostic;
using Enms.Client.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;

// FIXME: chart updates are always behind by one render

namespace Enms.Client.Shared.Charts;

#pragma warning disable S3881 // "IDisposable" should be implemented correctly
public partial class LineGraph : EnmsOwningComponentBase
#pragma warning restore S3881 // "IDisposable" should be implemented correctly
{
  private ApexChart<IMeasurement>? _chart = default!;

  private PaginatedList<IMeasurement> _measurements = new(
    new List<IMeasurement>(), 0);

  private ApexChartOptions<IMeasurement> _options =
    NewApexChartOptions<IMeasurement>();

  [Parameter]
  public ILine Model { get; set; } = default!;

  [Parameter]
  public DateTimeOffset? Timestamp { get; set; } = default!;

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

  protected override void OnInitialized()
  {
    MeasurementSubscriber.SubscribeUpsert(OnUpsert);

    _options = CreateGraphOptions();
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
    var now = DateTimeOffset.UtcNow;
    var timestamp = Timestamp ?? now;
    var timeSpan = Resolution
      .ToTimeSpan(Multiplier, timestamp);
    var appropriateInterval = QueryConstants
      .AppropriateInterval(timeSpan, timestamp);

    if (appropriateInterval is null)
    {
      var measurementQueries = ScopedServices
        .GetRequiredService<MeasurementQueries>();
      var measurements = await measurementQueries.ReadDynamic(
        timestamp.Subtract(timeSpan),
        timestamp,
        lineId: Model.LineId,
        meterId: Model.MeterId
      );
      _measurements = measurements;
    }
    else
    {
      var aggregateQueries = ScopedServices
        .GetRequiredService<AggregateQueries>();
      var aggregates = await aggregateQueries.ReadDynamic(
        timestamp.Subtract(timeSpan),
        timestamp,
        interval: appropriateInterval,
        lineId: Model.LineId,
        meterId: Model.MeterId
      );
      var casted = new PaginatedList<IMeasurement>(
        aggregates.Items.Cast<IMeasurement>().ToList(),
        aggregates.TotalCount
      );
      _measurements = casted;
    }

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
      var timestamp = _measurements.Items.LastOrDefault()?.Timestamp ?? Timestamp ?? now;
      var timeSpan = Resolution.ToTimeSpan(Multiplier, timestamp);
      var appropriateInterval = QueryConstants.AppropriateInterval(timeSpan, now);

      if (appropriateInterval is null)
      {
        var newMeasurements = args.Measurements
          .Where(x => x.Timestamp >= timestamp)
          .Where(x => x.LineId == Model.LineId)
          .Where(x => x.MeterId == Model.MeterId)
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
          .Where(x => x.LineId == Model.LineId)
          .Where(x => x.MeterId == Model.MeterId)
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
    var measure = $"{Translate(Measure.ToTitle())} ({Measure.ToUnit()})";
    var maxPower = _measurements.Items
      .OrderByDescending(
        m => m.ActivePower_W.TariffUnary().DuplexImport().PhaseSum())
      .FirstOrDefault()
      ?.ActivePower_W.TariffUnary().DuplexImport().PhaseSum();

    var options = SetPowerAnnotationGraphOptions(
      null,
      Translate("CONNECTION POWER"),
      Model.ConnectionPower_W,
      maxPower
    );

    if (Breakpoint <= Breakpoint.Sm)
    {
      options = SetSmAndDownGraphOptions(
        options,
        measure
      );
    }
    else
    {
      options = SetMdAndUpGraphOptions(
        options,
        measure
      );
    }

    return options;
  }

  private static ApexChartOptions<IMeasurement> SetPowerAnnotationGraphOptions(
    ApexChartOptions<IMeasurement>? options,
    string label,
    decimal connectionPower,
    decimal? maxPower)
  {
    options ??= NewApexChartOptions<IMeasurement>();

    if (maxPower is null)
    {
      options.Yaxis =
      [
        new YAxis
        {
          Max = maxPower * 1.5M,
          Labels = new YAxisLabels
          {
            Formatter = "function(val, index) { return (val ?? 0).toFixed(0); }"
          }
        }
      ];
      options.Annotations = new Annotations
      {
        Yaxis = [CreateYAxisAnnotations(label, connectionPower)]
      };
    }
    else
    {
      options.Annotations = new Annotations();
      options.Yaxis =
      [
        new YAxis
        {
          Labels = new YAxisLabels
          {
            Formatter = "function(val, index) { return (val ?? 0).toFixed(0); }"
          }
        }
      ];
    }

    return options;
  }

  private static ApexChartOptions<IMeasurement> SetSmAndDownGraphOptions(
    ApexChartOptions<IMeasurement>? options,
    string measure
  )
  {
    options ??= NewApexChartOptions<IMeasurement>();
    options.Grid = new Grid
    {
      BorderColor = "#e7e7e7",
      Row = new GridRow
      {
        Colors = new List<string> { "#ddeeff", "transparent" },
        Opacity = 0.5d
      }
    };
    options.Tooltip = new Tooltip
    {
      X = new TooltipX { Format = @"HH:mm:ss" },
      Y = new TooltipY
      {
        Title = new TooltipYTitle
        {
          Formatter = $"function(name) {{ return '{measure} ' + name; }}"
        }
      }
    };
    options.Yaxis =
    [
      new YAxis
      {
        Labels = new YAxisLabels
        {
          Formatter = "function(val, index) { return (val ?? 0).toFixed(0); }"
        }
      }
    ];
    options.Xaxis = new XAxis
    {
      Labels = new XAxisLabels { Show = false },
    };
    options.Chart = new Chart
    {
      Toolbar = new Toolbar
      {
        Tools = new Tools
        {
          Zoomin = false,
          Zoomout = false,
          Download = false,
          Pan = false,
          Selection = false,
          Reset = false
        }
      }
    };

    return options;
  }

  private static ApexChartOptions<IMeasurement> SetMdAndUpGraphOptions(
    ApexChartOptions<IMeasurement>? options,
    string measure
  )
  {
    options ??= NewApexChartOptions<IMeasurement>();
    options.Grid = new Grid
    {
      BorderColor = "#e7e7e7",
      Row = new GridRow
      {
        Colors = new List<string> { "#ddeeff", "transparent" },
        Opacity = 0.5d
      }
    };
    options.Chart = new Chart
    {
      Toolbar = new Toolbar
      {
        Tools = new Tools
        {
          Zoomin = false,
          Zoomout = false,
          Zoom = false,
          Download = false,
          Pan = false,
          Selection = false,
          Reset = false
        }
      }
    };
    options.Tooltip = new Tooltip
    {
      X = new TooltipX { Format = @"HH:mm:ss" },
      Y = new TooltipY
      {
        Title = new TooltipYTitle
        {
          Formatter = $"function(name) {{ return '{measure} ' + name; }}"
        }
      }
    };
    options.Yaxis =
    [
      new YAxis
      {
        Labels = new YAxisLabels
        {
          Formatter = "function(val, index) { return (val ?? 0).toFixed(0); }"
        }
      }
    ];
    options.Xaxis = new XAxis
    {
      Type = XAxisType.Datetime,
      AxisTicks = new AxisTicks(),
    };

    return options;
  }

  private static AnnotationsYAxis CreateYAxisAnnotations(
    string label,
    decimal connectionPower
  )
  {
    return new AnnotationsYAxis
    {
      Label = new Label
      {
        Text = label,
        Style = new Style
        {
          Background = "red",
          Color = "white",
          FontSize = "12px"
        }
      },
      Y = connectionPower * 3,
      BorderColor = "red",
      StrokeDashArray = 0
    };
  }
}
