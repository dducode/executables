using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Core.Events;
using Interactions.Policies;

namespace Interactions.Extensions;

public static class EventsExtensions {

  [Pure]
  public static IEvent<T> WithPolicies<T>(this IEvent<T> e, Policy<T, Unit> publishPolicy, Policy<ISubscriber<T>, IDisposable> subscriptionPolicy) {
    ExceptionsHelper.ThrowIfNullReference(e);
    ExceptionsHelper.ThrowIfNull(publishPolicy, nameof(publishPolicy));
    ExceptionsHelper.ThrowIfNull(subscriptionPolicy, nameof(subscriptionPolicy));
    return new PolicyEvent<T>(e, publishPolicy, subscriptionPolicy);
  }

  [Pure]
  public static IEvent<T> WithPublishPolicy<T>(this IEvent<T> e, Policy<T, Unit> publishPolicy) {
    return e.WithPolicies(publishPolicy, Policy<ISubscriber<T>, IDisposable>.Identity());
  }

  [Pure]
  public static IEvent<T> WithSubscriptionPolicy<T>(this IEvent<T> e, Policy<ISubscriber<T>, IDisposable> subscriptionPolicy) {
    return e.WithPolicies(Policy<T, Unit>.Identity(), subscriptionPolicy);
  }

}