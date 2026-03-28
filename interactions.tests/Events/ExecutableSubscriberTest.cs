using Interactions.Core.Executables;
using Interactions.Events;
using Interactions.Subscribers;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Interactions.Tests.Events;

[TestSubject(typeof(ExecutableSubscriber<>))]
public class ExecutableSubscriberTest(ITestOutputHelper output) {

  [Fact]
  public void CreateSubscriber() {
    ISubscriber<string> subscriber = Executable
      .Create((string message) => output.WriteLine(message))
      .AsSubscriber();

    subscriber.Receive("Test");
  }

  [Fact]
  public void ReceiveMessageFromEvent() {
    var e = new Event<string>();
    e.Subscribe(Executable
      .Create((string message) => output.WriteLine(message))
      .AsSubscriber()
    );

    e.Handle(EventPublisher.Sequential<string>());
    e.Publish("Test");
  }

}