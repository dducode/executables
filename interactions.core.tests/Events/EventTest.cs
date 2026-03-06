using Interactions.Core.Events;
using Interactions.Core.Extensions;
using Interactions.Core.Tests.Utils;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Events;

[TestSubject(typeof(Event<>))]
public class EventTest {

  [Fact]
  public void PublishWithoutSubscribers() {
    new Event<Unit>().Execute();
  }

  [Fact]
  public void PublishWithOnceSubscriberAndWithoutHandler() {
    var e = new Event<Unit>();
    e.Subscribe(() => { });
    Assert.Throws<MissingHandlerException>(() => e.Execute());
  }

  [Fact]
  public void PassNullSubscriber() {
    var e = new Event<Unit>();
    Assert.Throws<ArgumentNullException>(() => e.Subscribe(null));
  }

  [Fact]
  public void PassNullHandler() {
    var e = new Event<Unit>();
    Assert.Throws<ArgumentNullException>(() => e.Handle(null));
  }

  [Fact]
  public void UnsubscribedHandlersNotReceive() {
    var e = new Event<Unit>();
    var subscribers = new List<EventSubscriber>();
    for (var i = 0; i < 5; i++)
      subscribers.Add(new EventSubscriber());
    var disposableBag = new DisposableBag();
    foreach (EventSubscriber subscriber in subscribers)
      disposableBag.Add(e.Subscribe(subscriber));
    e.Handle(EventPublisher.Sequential());

    disposableBag.Dispose();
    e.Execute();
    Assert.All(subscribers, subscriber => Assert.False(subscriber.Received));
  }

  [Fact]
  public void Resubscription() {
    var e = new Event<Unit>();
    ISubscriber<Unit> subscriber = Subscriber.FromMethod<Unit>(_ => { });
    e.Subscribe(() => { });
    e.Subscribe(subscriber);
    Assert.Throws<InvalidOperationException>(() => e.Subscribe(subscriber));
  }

  [Fact]
  public async Task MultiplySubscriptionPublishingOnThreadPool() {
    var e = new Event<Unit>();
    var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));

    e.Handle(EventPublisher.Sequential());

    IEnumerable<Task> publishTasks = Enumerable.Repeat(Task.Run(async () => {
      while (!cts.Token.IsCancellationRequested) {
        e.Execute();
        await Task.Delay(1);
      }
    }), 10);

    IEnumerable<Task> subscribeTasks = Enumerable.Repeat(Task.Run(async () => {
      var random = new Random();

      while (!cts.Token.IsCancellationRequested) {
        using IDisposable subscription = e.Subscribe(() => { });
        await Task.Delay(random.Next(10, 30));
      }
    }), 10);

    await Task.WhenAll(publishTasks.Concat(subscribeTasks));
  }

}