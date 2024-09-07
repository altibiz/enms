using ApexCharts;
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
  public DateTimeOffset Timestamp { get; set; } = default!;

  [Parameter]
  public MeasureModel Measure { get; set; } = MeasureModel.ActivePower;

  [Parameter]
  public ResolutionModel Resolution { get; set; } = ResolutionModel.Minute;

  [Parameter]
  public HashSet<PhaseModel> Phases { get; set; } =
    Enum.GetValues<PhaseModel>().ToHashSet();

  [Parameter]
  public int Multiplier { get; set; } = 1;

  [Parameter]
  public bool Refresh { get; set; } = true;

  [CascadingParameter]
  public Breakpoint Breakpoint { get; set; } = default!;

  [Inject]
  public IMeasurementSubscriber MeasurementSubscriber { get; set; } = default!;

  private SemaphoreSlim _semaphore = new(1, 1);

  protected override void OnInitialized()
  {
    MeasurementSubscriber.SubscribeUpsert(OnUpsert);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing)
    {
      MeasurementSubscriber.UnsubscribeUpsert(OnUpsert);
    }
  }

  protected override void OnParametersSet()
  {
    var now = DateTimeOffset.UtcNow;
    if (Timestamp == default)
    {
      Timestamp = now.Subtract(Resolution.ToTimeSpan(Multiplier, now));
    }

    _options = CreateGraphOptions();
  }

  protected override async Task OnParametersSetAsync()
  {
    _measurements = await LoadAsync();
    Console.WriteLine($"Measurements: {_measurements.Items.Count}");
  }

  protected override async Task OnAfterRenderAsync(bool firstRender)
  {
    await _semaphore.WaitAsync();

    _measurements = await LoadAsync();
    _options = CreateGraphOptions();
    Console.WriteLine($"Measurements: {_measurements.Items.Count}");

    if (_chart is { } chart)
    {
      await chart.UpdateSeriesAsync(animate: false);
      await chart.UpdateOptionsAsync(true, false, false);
    }

    _semaphore.Release();
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
      await _semaphore.WaitAsync();

      var now = DateTimeOffset.UtcNow;
      Timestamp = now.Subtract(Resolution.ToTimeSpan(Multiplier, now));

      var timeSpan = Resolution.ToTimeSpan(Multiplier, Timestamp);
      var appropriateInterval = QueryConstants
        .AppropriateInterval(timeSpan, Timestamp);

      var newMeasurements = appropriateInterval is null
        ? args.Measurements
          .Where(x => x.Timestamp >= Timestamp)
          .Where(x => x.LineId == Model.LineId)
          .Where(x => x.MeterId == Model.MeterId)
          .OrderByDescending(x => x.Timestamp)
          .ToList()
        : args.Aggregates
          .Where(x => x.Timestamp >= Timestamp)
          .Where(x => x.Interval == appropriateInterval)
          .Where(x => x.LineId == Model.LineId)
          .Where(x => x.MeterId == Model.MeterId)
          .OrderByDescending(x => x.Timestamp)
          .OfType<IMeasurement>()
          .ToList();

      _measurements = new PaginatedList<IMeasurement>(
        _measurements.Items.Concat(newMeasurements).ToList(),
        _measurements.TotalCount + newMeasurements.Count
      );
      Console.WriteLine($"New measurements: {newMeasurements.Count}");

      _options = CreateGraphOptions(_measurements.Items
        .OrderByDescending(x => x.Timestamp)
        .FirstOrDefault()?.Timestamp);
      if (_chart is { } chart)
      {
        await chart.AppendDataAsync(newMeasurements);
        await chart.UpdateOptionsAsync(false, true, false);
      }

      _semaphore.Release();
    });
  }

  private async Task<PaginatedList<IMeasurement>> LoadAsync()
  {
    var timeSpan = Resolution.ToTimeSpan(Multiplier, Timestamp);
    var appropriateInterval = QueryConstants
      .AppropriateInterval(timeSpan, Timestamp);

    if (appropriateInterval is null)
    {
      var measurementQueries = ScopedServices
        .GetRequiredService<MeasurementQueries>();
      var measurements = await measurementQueries.ReadDynamic(
        Timestamp,
        Timestamp.Add(timeSpan),
        lineId: Model.LineId,
        meterId: Model.MeterId
      );
      return measurements;
    }

    var aggregateQueries = ScopedServices
      .GetRequiredService<AggregateQueries>();
    var aggregates = await aggregateQueries.ReadDynamic(
      Timestamp,
      Timestamp.Add(timeSpan),
      interval: appropriateInterval,
      lineId: Model.LineId,
      meterId: Model.MeterId
    );
    var casted = new PaginatedList<IMeasurement>(
      aggregates.Items.Cast<IMeasurement>().ToList(),
      aggregates.TotalCount
    );
    return casted;
  }

  private ApexChartOptions<IMeasurement> CreateGraphOptions(
    DateTimeOffset? max = null
  )
  {
    var measure = $"{Translate(Measure.ToTitle())} ({Measure.ToUnit()})";
    max ??= Timestamp;
    var min = max.Value.Add(-Resolution.ToTimeSpan(Multiplier, max.Value));
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
      options = SetSmAndDownTimeRangeGraphOptions(
        options,
        min,
        max.Value
      );
    }
    else
    {
      options = SetMdAndUpGraphOptions(
        options,
        measure
      );
      options = SetMdAndUpTimeRangeGraphOptions(
        options,
        min,
        max.Value
      );
    }

    return options;
  }

  private static ApexChartOptions<IMeasurement>
    SetSmAndDownTimeRangeGraphOptions(
      ApexChartOptions<IMeasurement>? options,
      DateTimeOffset min,
      DateTimeOffset max
    )
  {
    options ??= NewApexChartOptions<IMeasurement>();

    options.Xaxis = new XAxis
    {
      Labels = new XAxisLabels { Show = false },
      Min = DateTimeChart(min),
      Max = DateTimeChart(max)
    };

    return options;
  }

  private static ApexChartOptions<IMeasurement> SetMdAndUpTimeRangeGraphOptions(
    ApexChartOptions<IMeasurement>? options,
    DateTimeOffset min,
    DateTimeOffset max
  )
  {
    options ??= NewApexChartOptions<IMeasurement>();

    options.Xaxis = new XAxis
    {
      Type = XAxisType.Datetime,
      AxisTicks = new AxisTicks(),
      Min = DateTimeChart(min),
      Max = DateTimeChart(max)
    };

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
