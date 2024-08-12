using ApexCharts;
using Enms.Business.Models;
using Enms.Business.Models.Abstractions;
using Enms.Business.Queries.Abstractions;
using Enms.Business.Queries.Agnostic;
using Enms.Client.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Enms.Client.Shared.Charts;

public partial class LineGraph : EnmsOwningComponentBase
{
  [Parameter]
  public ILine Model { get; set; } = default!;

  [Parameter]
  public DateTimeOffset Timestamp { get; set; } = default!;

  [Parameter]
  public ChartMeasure Measure { get; set; } = ChartMeasure.ActivePower;

  [Parameter]
  public ChartResolution Resolution { get; set; } = ChartResolution.Minute;

  [Parameter]
  public HashSet<PhaseModel> Phases { get; set; } =
    Enum.GetValues<PhaseModel>().ToHashSet();

  [Parameter]
  public int Multiplier { get; set; } = 1;

  [Parameter]
  public bool Refresh { get; set; } = true;

  private ApexChart<IMeasurement> _chart = default!;

  private ApexChartOptions<IMeasurement> _options =
    NewApexChartOptions<IMeasurement>();

  private AnnotationsYAxis _annotations = new();

  protected override void OnParametersSet()
  {
    var now = DateTimeOffset.UtcNow;
    if (Timestamp == default)
    {
      Timestamp = now.Subtract(Resolution.ToTimeSpan(Multiplier, now));
    }
  }

  protected override void OnInitialized()
  {
    _options = CreateMdAndUpGraphOptions();
    _annotations = CreateYAxisAnnotations();
  }

  private async Task<PaginatedList<IMeasurement>> LoadAsync()
  {
    var timeSpan = Resolution.ToTimeSpan(Multiplier, Timestamp);
    var appropriateInterval = QueryConstants
      .AppropriateInterval(
        timeSpan,
        Timestamp,
        Multiplier
      );

    if (appropriateInterval is null)
    {
      var measurementQueries = ScopedServices
        .GetRequiredService<MeasurementQueries>();
      var measurements = await measurementQueries.ReadDynamic(
        Timestamp,
        Timestamp.Add(timeSpan),
        lineId: Model.Id
      );
      return measurements;
    }

    var aggregateQueries = ScopedServices
      .GetRequiredService<AggregateQueries>();
    var aggregates = await aggregateQueries.ReadDynamic(
      Timestamp,
      Timestamp.Add(timeSpan),
      interval: appropriateInterval,
      lineId: Model.Id
    );
    var casted = new PaginatedList<IMeasurement>(
      aggregates.Items.Cast<IMeasurement>().ToList(),
      aggregates.TotalCount
    );
    return casted;
  }

  private ApexChartOptions<IMeasurement> CreateSmAndDownGraphOptions()
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
    options.Yaxis = new List<YAxis>();
    options.Yaxis.Add(
      new YAxis
      {
        Labels = new YAxisLabels
        {
          Formatter = "function(val, index) { return (val ?? 0).toFixed(0); }"
        }
      });
    options.Xaxis = new XAxis();
    options.Xaxis = new XAxis
    {
      Labels = new XAxisLabels { Show = false },
      Range = Resolution.ToTimeSpan(Multiplier, Timestamp).TotalMilliseconds
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

  private ApexChartOptions<IMeasurement> CreateMdAndUpGraphOptions()
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
    options.Yaxis = new List<YAxis>();
    options.Yaxis.Add(
      new YAxis
      {
        Labels = new YAxisLabels
        {
          Formatter = "function(val, index) { return (val ?? 0).toFixed(0); }"
        }
      });
    options.Xaxis = new XAxis();
    options.Xaxis = new XAxis
    {
      Type = XAxisType.Datetime,
      AxisTicks = new AxisTicks(),
      Range = Resolution.ToTimeSpan(Multiplier, Timestamp).TotalMilliseconds
    };

    return options;
  }

  private AnnotationsYAxis CreateYAxisAnnotations()
  {
    var annotations = new AnnotationsYAxis
    {
      Label = new Label
      {
        Text = Translate("CONNECTION POWER"),
        Style = new Style
        {
          Background = "red",
          Color = "white",
          FontSize = "12px"
        }
      },
      Y = Model.ConnectionPower_W * 3,
      BorderColor = "red",
      StrokeDashArray = 0
    };

    return annotations;
  }

  private async Task SetAnnotationGraphOptions()
  {
    if (Measure == ChartMeasure.ActivePower)
    {

    }

    if (_dataTitle == "Active Power")
    {
      if (_graphValues is not null && _graphValues.Any() && _graphValues.All(x => x.values.Length > 3))
      {
        var graphMaxPower = _graphValues.Select(x => x.values[3]).Max();
        graphOptions.Yaxis.Clear();
        graphOptions.Yaxis.Add(
          new YAxis
          {
            Max = graphMaxPower * 1.5M,
            Labels = new YAxisLabels
            {
              Formatter = "function(val, index) { return (val ?? 0).toFixed(0); }"
            }
          });
        graphOptions.Annotations = new Annotations
        {
          Yaxis = new List<AnnotationsYAxis> { _annotation }
        };

        graphOptionsMob.Yaxis.Clear();
        graphOptionsMob.Yaxis.Add(
          new YAxis
          {
            Max = graphMaxPower * 1.5M,
            Labels = new YAxisLabels
            {
              Formatter = "function(val, index) { return (val ?? 0).toFixed(0); }"
            }
          });
        graphOptionsMob.Annotations = new Annotations
        {
          Yaxis = new List<AnnotationsYAxis> { _annotation }
        };

        if (chart is not null)
        {
          await chart.RenderAsync();
          await chart.AddYAxisAnnotationAsync(_annotation, true);
        }
      }
    }
    else
    {
      graphOptions.Annotations = new Annotations();
      graphOptions.Yaxis.Clear();
      graphOptions.Yaxis.Add(
        new YAxis
        {
          Labels = new YAxisLabels
          {
            Formatter = "function(val, index) { return (val ?? 0).toFixed(0); }"
          }
        });

      graphOptionsMob.Annotations = new Annotations();
      graphOptionsMob.Yaxis.Clear();
      graphOptionsMob.Yaxis.Add(
        new YAxis
        {
          Labels = new YAxisLabels
          {
            Formatter = "function(val, index) { return (val ?? 0).toFixed(0); }"
          }
        });

      if (chart is not null)
      {
        await chart.ClearAnnotationsAsync();
        await chart.RenderAsync();
      }
    }
  }
  private void SetGraphTimeRange()
  {
    graphOptions.Xaxis = new XAxis
    {
      Type = XAxisType.Datetime,
      AxisTicks = new AxisTicks(),
      Range = 60000 * timeSpanMins
    };

    graphOptionsMob.Xaxis = new XAxis
    {
      Labels = new XAxisLabels { Show = false },
      Range = 60000 * timeSpanMins
    };
  }


  private async Task UpdateChartSeries()
  {
    if (_graphValues is not null)
    {
      var lastGraphValues = _graphValues.OrderByDescending(x => x.date).FirstOrDefault();
      if (chart is null)
      {
        return;
      }

      if (lastGraphValues is not null)
      {
        await GetValues(lastGraphValues.date, DateTimeOffset.UtcNow.LocalDateTime, false);
        await chart.AppendDataAsync(_graphValues);
      }
    }
  }
}
