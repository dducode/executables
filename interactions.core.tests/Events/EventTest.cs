using Interactions.Core.Events;
using Interactions.Core.Extensions;
using Interactions.Core.Tests.Utils;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Events;

[TestSubject(typeof(Event<>))]
public class EventTest {

  [Fact]
  public void PublishWithoutSubscribers() {
    new Event<Unit>().Publish();
  }

  [Fact]
  public void SimplePublish() {
    var e = new Event<Unit>();
    var subscribers = new List<EventSubscriber>();
    for (var i = 0; i < 5; i++)
      subscribers.Add(new EventSubscriber());
    foreach (EventSubscriber subscriber in subscribers)
      e.Handle(subscriber);

    e.Publish();
    Assert.All(subscribers, subscriber => Assert.True(subscriber.Receive));
  }

  [Fact]
  public void PassNullSubscriber() {
    var e = new Event<Unit>();
    Assert.Throws<ArgumentNullException>(() => e.Handle(null));
  }

  [Fact]
  public void OneSubscriberThrowsException() {
    var e = new Event<Unit>();
    var subscribers = new List<EventSubscriber> {
      new(),
      new ThrowingExceptionSubscriber(),
      new()
    };

    foreach (EventSubscriber subscriber in subscribers)
      e.Handle(subscriber);

    Assert.Throws<InvalidOperationException>(() => e.Publish());
    Assert.All(subscribers, subscriber => Assert.True(subscriber.Receive));
  }

  [Fact]
  public void TwoSubscribersThrowsException() {
    var e = new Event<Unit>();
    var subscribers = new List<EventSubscriber> {
      new(),
      new ThrowingExceptionSubscriber(),
      new(),
      new ThrowingExceptionSubscriber(),
      new()
    };

    foreach (EventSubscriber subscriber in subscribers)
      e.Handle(subscriber);

    Assert.Throws<AggregateException>(() => e.Publish());
    Assert.All(subscribers, subscriber => Assert.True(subscriber.Receive));
  }

  [Fact]
  public void UnsubscribedHandlersNotReceive() {
    var e = new Event<Unit>();
    var subscribers = new List<EventSubscriber>();
    for (var i = 0; i < 5; i++)
      subscribers.Add(new EventSubscriber());
    var disposableBag = new DisposableBag();
    foreach (EventSubscriber subscriber in subscribers)
      disposableBag.Add(e.Handle(subscriber));

    disposableBag.Dispose();
    e.Publish();
    Assert.All(subscribers, subscriber => Assert.False(subscriber.Receive));
  }

  [Fact]
  public void Resubscription() {
    var e = new Event<Unit>();
    Handler<Unit, Unit> handler = Handler.Identity();
    e.Handle(Handler.Identity());
    e.Handle(handler);
    Assert.Throws<InvalidOperationException>(() => e.Handle(handler));
  }

  [Fact]
  public async Task MultiplySubscriptionPublishingOnThreadPool() {
    var e = new Event<Unit>();
    var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));

    IEnumerable<Task> publishTasks = Enumerable.Repeat(Task.Run(async () => {
      while (!cts.Token.IsCancellationRequested) {
        e.Publish();
        await Task.Delay(1);
      }
    }), 10);

    IEnumerable<Task> subscribeTasks = Enumerable.Repeat(Task.Run(async () => {
      var random = new Random();

      while (!cts.Token.IsCancellationRequested) {
        using IDisposable handle = e.Handle(Handler.Identity());
        await Task.Delay(random.Next(10, 30));
      }
    }), 10);

    await Task.WhenAll(publishTasks.Concat(subscribeTasks));
  }

}