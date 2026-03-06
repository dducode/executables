using Interactions.Core;
using Interactions.Core.Events;

namespace Interactions.Policies;

internal sealed class PolicyEvent<T>(IEvent<T> inner, Policy<T, Unit> publishPolicy, Policy<ISubscriber<T>, IDisposable> subscriptionPolicy) : IEvent<T> {

  public void Publish(T input) {
    publishPolicy.Execute(input, i => {
      inner.Publish(i);
      return default;
    });
  }

  public IDisposable Subscribe(ISubscriber<T> subscriber) {
    return subscriptionPolicy.Execute(subscriber, inner.Subscribe);
  }

}