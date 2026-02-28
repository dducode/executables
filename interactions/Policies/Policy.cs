using Interactions.Core;

namespace Interactions.Policies;

/// <summary>
/// Defines a synchronous policy that can wrap query or command invocation.
/// </summary>
/// <typeparam name="T1">Input value type.</typeparam>
/// <typeparam name="T2">Result value type.</typeparam>
public abstract partial class Policy<T1, T2> {

  /// <summary>
  /// Executes the policy around provided invocation.
  /// </summary>
  /// <param name="input">Input passed to policy and invocation.</param>
  /// <param name="invocation">Wrapped operation that produces a result.</param>
  /// <returns>Operation result after policy logic is applied.</returns>
  public abstract T2 Execute(T1 input, Func<T1, T2> invocation);

}

/// <summary>
/// Defines an asynchronous policy that can wrap query or command invocation.
/// </summary>
/// <typeparam name="T1">Input value type.</typeparam>
/// <typeparam name="T2">Result value type.</typeparam>
public abstract partial class AsyncPolicy<T1, T2> {

  /// <summary>
  /// Executes the policy around provided asynchronous invocation.
  /// </summary>
  /// <param name="input">Input passed to policy and invocation.</param>
  /// <param name="invocation">Wrapped asynchronous operation.</param>
  /// <param name="token">Cancellation token used by policy and invocation.</param>
  /// <returns>A task that resolves to the invocation result after policy logic is applied.</returns>
  public abstract ValueTask<T2> Execute(T1 input, AsyncFunc<T1, T2> invocation, CancellationToken token);

}
