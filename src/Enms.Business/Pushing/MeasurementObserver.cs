using Enms.Business.Models.Abstractions;
using Enms.Business.Pushing.Abstractions;

namespace Enms.Business.Pushing;

public class MeasurementPublisher(ILogger<MeasurementPublisher> logger)
  : IMeasurementPublisher, IMeasurementSubscriber
{
  public void AfterPush(
    IReadOnlyList<IMeasurement> measurements,
    IReadOnlyList<IAggregate> aggregates)
  {
    try
    {
      OnAfterPublish?.Invoke(
        this,
        new MeasurementPublishEventArgs(measurements, aggregates)
      );
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "AfterPublish");
    }
  }

  public void BeforePush(
    IReadOnlyList<IMeasurement> measurements,
    IReadOnlyList<IAggregate> aggregates)
  {
    try
    {
      OnBeforePublish?.Invoke(
        this,
        new MeasurementPublishEventArgs(measurements, aggregates)
      );
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "BeforePublish");
    }
  }

  public void SubscribeAfterPush(
    EventHandler<MeasurementPublishEventArgs> handler)
  {
    OnAfterPublish += handler;
  }

  public void SubscribeBeforePush(
    EventHandler<MeasurementPublishEventArgs> handler)
  {
    OnBeforePublish += handler;
  }

  public void UnsubscribeAfterPush(
    EventHandler<MeasurementPublishEventArgs> handler)
  {
    OnAfterPublish -= handler;
  }

  public void UnsubscribeBeforePush(
    EventHandler<MeasurementPublishEventArgs> handler)
  {
    OnBeforePublish -= handler;
  }

  public event EventHandler<MeasurementPublishEventArgs>? OnBeforePublish;

  public event EventHandler<MeasurementPublishEventArgs>? OnAfterPublish;
}
