using System.Diagnostics.Contracts;
using Interactions.Commands;
using Interactions.Core;
using Interactions.Core.Commands;
using Interactions.Policies;

namespace Interactions.Extensions;

public static class CommandsExtensions {

  [Pure]
  public static Command<T> Compose<T>(this Command<T> command, Command<T> other) {
    ExceptionsHelper.ThrowIfNull(other, nameof(other));
    return new CompositeCommand<T>(command, other);
  }

  [Pure]
  public static AsyncCommand<T> Compose<T>(this AsyncCommand<T> command, AsyncCommand<T> other) {
    ExceptionsHelper.ThrowIfNull(other, nameof(other));
    return new AsyncCompositeCommand<T>(command, other);
  }

  [Pure]
  public static ICommand<T> WithPolicy<T>(this ICommand<T> command, Policy<T, bool> policy) {
    ExceptionsHelper.ThrowIfNull(policy, nameof(policy));
    return new PolicyCommand<T>(command, policy);
  }

  [Pure]
  public static IAsyncCommand<T> WithPolicy<T>(this IAsyncCommand<T> command, AsyncPolicy<T, bool> policy) {
    ExceptionsHelper.ThrowIfNull(policy, nameof(policy));
    return new AsyncPolicyCommand<T>(command, policy);
  }

}