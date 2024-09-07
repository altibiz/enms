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
    if (_chart is { } chart)
    {
      await chart.UpdateSeriesAsync();
    }
  }

  protected override async Task OnAfterRenderAsync(bool firstRender)
  {
    if (firstRender && _chart is { } chart)
    {
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

    Task.Run(
      async () =>
      {
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
            .ToList()
          : args.Aggregates
            .Where(x => x.Timestamp >= Timestamp)
            .Where(x => x.Interval == appropriateInterval)
            .Where(x => x.LineId == Model.LineId)
            .Where(x => x.MeterId == Model.MeterId)
            .OfType<IMeasurement>()
            .ToList();

        _measurements = new PaginatedList<IMeasurement>(
          _measurements.Items.Concat(newMeasurements).ToList(),
          _measurements.TotalCount + newMeasurements.Count
        );

        _options = CreateGraphOptions();
        if (_chart is null)
        {
          return;
        }

        await _chart.AppendDataAsync(newMeasurements);
        await _chart.UpdateOptionsAsync(false, true, false);
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

  private ApexChartOptions<IMeasurement> CreateGraphOptions()
  {
    if (Breakpoint <= Breakpoint.Sm)
    {
      var options = CreateSmAndDownGraphOptions(
        Resolution,
        Timestamp,
        Multiplier
      );
      options = SetPowerAnnotationGraphOptions(
        options,
        Translate("CONNECTION POWER"),
        Model.ConnectionPower_W,
        _measurements.Items
          .OrderByDescending(
            m => m.ActivePower_W.TariffUnary().DuplexImport().PhaseSum())
          .FirstOrDefault()
          ?.ActivePower_W.TariffUnary().DuplexImport().PhaseSum());
      options = SetSmAndDownTimeRangeGraphOptions(
        options,
        Resolution,
        Timestamp,
        Multiplier
      );
      return options;
    }
    else
    {
      var options = CreateMdAndUpGraphOptions(
        Resolution,
        Timestamp,
        Multiplier
      );
      options = SetPowerAnnotationGraphOptions(
        options,
        Translate("CONNECTION POWER"),
        Model.ConnectionPower_W,
        _measurements.Items
          .OrderByDescending(
            m => m.ActivePower_W.TariffUnary().DuplexImport().PhaseSum())
          .FirstOrDefault()
          ?.ActivePower_W.TariffUnary().DuplexImport().PhaseSum());
      options = SetMdAndUpTimeRangeGraphOptions(
        options,
        Resolution,
        Timestamp,
        Multiplier
      );
      return options;
    }
  }

  private static ApexChartOptions<IMeasurement>
    SetSmAndDownTimeRangeGraphOptions(
      ApexChartOptions<IMeasurement> options,
      ResolutionModel resolution,
      DateTimeOffset timestamp,
      int multiplier
    )
  {
    options.Xaxis = new XAxis
    {
      Labels = new XAxisLabels { Show = false },
      Range = resolution.ToTimeSpan(multiplier, timestamp).TotalMilliseconds
    };

    return options;
  }

  private static ApexChartOptions<IMeasurement> SetMdAndUpTimeRangeGraphOptions(
    ApexChartOptions<IMeasurement> options,
    ResolutionModel resolution,
    DateTimeOffset timestamp,
    int multiplier
  )
  {
    options.Xaxis = new XAxis
    {
      Type = XAxisType.Datetime,
      AxisTicks = new AxisTicks(),
      Range = resolution.ToTimeSpan(multiplier, timestamp).TotalMilliseconds
    };

    return options;
  }

  private static ApexChartOptions<IMeasurement> SetPowerAnnotationGraphOptions(
    ApexChartOptions<IMeasurement> options,
    string label,
    decimal connectionPower,
    decimal? maxPower)
  {
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

  private static ApexChartOptions<IMeasurement> CreateSmAndDownGraphOptions(
    ResolutionModel resolution,
    DateTimeOffset timestamp,
    int multiplier
  )
  {
    var options = NewApexChartOptions<IMeasurement>();
    options.Grid = new Grid
    {
      BorderColor = "#e7e7e7",
      Row = new GridRow
      {
        Colors = new List<string> { "#f3f3f3", "transparent" },
        Opacity = 0.5d
      }
    };
    options.Tooltip = new Tooltip
    {
      X = new TooltipX { Format = @"HH:mm:ss" }
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
      Range = resolution.ToTimeSpan(multiplier, timestamp).TotalMilliseconds
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
          Selection = false
        }
      }
    };

    return options;
  }

  private static ApexChartOptions<IMeasurement> CreateMdAndUpGraphOptions(
    ResolutionModel resolution,
    DateTimeOffset timestamp,
    int multiplier
  )
  {
    var options = NewApexChartOptions<IMeasurement>();
    options.Grid = new Grid
    {
      BorderColor = "#e7e7e7",
      Row = new GridRow
      {
        Colors = new List<string> { "#f3f3f3", "transparent" },
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
          Pan = true,
          Selection = false
        }
      }
    };
    options.Tooltip = new Tooltip
    {
      X = new TooltipX { Format = @"HH:mm:ss" }
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
      Range = resolution.ToTimeSpan(multiplier, timestamp).TotalMilliseconds
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
