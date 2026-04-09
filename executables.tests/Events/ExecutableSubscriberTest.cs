using Executables.Core.Executables;
using Executables.Events;
using Executables.Subscribers;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Executables.Tests.Events;

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