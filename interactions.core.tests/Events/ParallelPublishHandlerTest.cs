using Interactions.Core.Events;
using Interactions.Core.Extensions;
using Interactions.Core.Tests.Utils;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Interactions.Core.Tests.Events;

[TestSubject(typeof(ParallelPublishHandler<>))]
public class ParallelPublishHandlerTest(ITestOutputHelper output) {

  [Fact]
  public void ParallelPublishing() {
    var e = new Event<Unit>();
    var subscribers = new List<EventSubscriber>();
    for (var i = 0; i < 5; i++) {
      int index = i + 1;
      subscribers.Add(new EventSubscriber(() => output.WriteLine($"Publisher: {index}")));
    }

    foreach (EventSubscriber subscriber in subscribers)
      e.Subscribe(subscriber);
    e.Handle(EventPublisher.Parallel());

    e.Execute();
    Assert.All(subscribers, subscriber => Assert.True(subscriber.Received));
  }

  [Fact]
  public void OneSubscriberThrowsException() {
    var e = new Event<Unit>();
    var subscribers = new List<EventSubscriber> {
      new(),
      new(() => throw new InvalidOperationException("Exception from subscriber")),
      new()
    };

    foreach (EventSubscriber subscriber in subscribers)
      e.Subscribe(subscriber);
    e.Handle(EventPublisher.Parallel());

    Assert.Throws<InvalidOperationException>(() => e.Execute());
    Assert.All(subscribers, subscriber => Assert.True(subscriber.Received));
  }

  [Fact]
  public void TwoSubscribersThrowsException() {
    var e = new Event<Unit>();
    var subscribers = new List<EventSubscriber> {
      new(),
      new(() => throw new InvalidOperationException("Exception from subscriber")),
      new(),
      new(() => throw new InvalidOperationException("Exception from subscriber")),
      new()
    };

    foreach (EventSubscriber subscriber in subscribers)
      e.Subscribe(subscriber);
    e.Handle(EventPublisher.Parallel());

    Assert.Throws<AggregateException>(() => e.Execute());
    Assert.All(subscribers, subscriber => Assert.True(subscriber.Received));
  }

}