using Interactions.Core.Events;
using Interactions.Events;
using Interactions.Tests.Utils;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Interactions.Tests.Core.Events;

[TestSubject(typeof(SequentialPublisher<>))]
public class SequentialPublisherTest(ITestOutputHelper output) {

  [Theory]
  [InlineData(PublishOrder.Direct)]
  [InlineData(PublishOrder.Reverse)]
  public void SequentialPublishing(PublishOrder order) {
    var e = new Event<Unit>();
    var subscribers = new List<EventSubscriber>();
    for (var i = 0; i < 5; i++) {
      int index = i + 1;
      subscribers.Add(new EventSubscriber(() => output.WriteLine($"Publisher: {index}")));
    }

    foreach (EventSubscriber subscriber in subscribers)
      e.Subscribe(subscriber);
    e.Handle(EventPublisher.Sequential(order));

    e.Publish();
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
    e.Handle(EventPublisher.Sequential());

    Assert.Throws<InvalidOperationException>(() => e.Publish());
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
    e.Handle(EventPublisher.Sequential());

    Assert.Throws<AggregateException>(() => e.Publish());
    Assert.All(subscribers, subscriber => Assert.True(subscriber.Received));
  }

}