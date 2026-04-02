using System.Diagnostics.Contracts;
using Executables.Analytics;
using Executables.Context;
using Executables.Core.Executables;
using Executables.Core.Operators;
using Executables.Handling;
using Executables.Internal;
using Executables.Operations;
using Executables.Policies;
using Executables.Subscribers;

namespace Executables;

public static class ExecutableExtensions {

  /// <summary>
  /// Returns the same executable instance.
  /// </summary>
  [Pure]
  public static IExecutable<T1, T2> AsExecutable<T1, T2>(this IExecutable<T1, T2> executable) {
    executable.ThrowIfNullReference();
    return executable;
  }

  /// <summary>
  /// Converts a synchronous executable to an asynchronous executable.
  /// </summary>
  /// <returns>Asynchronous proxy executable.</returns>
  [Pure]
  public static IAsyncExecutable<T1, T2> ToAsyncExecutable<T1, T2>(this IExecutable<T1, T2> executable) {
    executable.ThrowIfNullReference();
    return new AsyncProxyExecutable<T1, T2>(executable);
  }

  /// <summary>
  /// Composes two executables into a single pipeline.
  /// </summary>
  /// <param name="first">Executable invoked first.</param>
  /// <param name="second">Executable invoked with the result of <paramref name="first"/>.</param>
  /// <returns>Composed executable.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, T3> Then<T1, T2, T3>(this IExecutable<T1, T2> first, IExecutable<T2, T3> second) {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
#if !DEBUG
    if (first is IdentityExecutable<T1>)
      return (IExecutable<T1, T3>)second;
#endif
    return new CompositeExecutable<T1, T2, T3>(first, second);
  }

  /// <summary>
  /// Appends a delegate to an executable pipeline.
  /// </summary>
  /// <param name="executable">Executable invoked first.</param>
  /// <param name="next">Delegate invoked with the result of <paramref name="executable"/>.</param>
  /// <returns>Composed executable.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="next"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, T3> Then<T1, T2, T3>(this IExecutable<T1, T2> executable, Func<T2, T3> next) {
    return executable.Then(Executable.Create(next));
  }

  /// <summary>
  /// Appends a parameterless delegate to an executable pipeline.
  /// </summary>
  /// <param name="executable">Executable invoked first.</param>
  /// <param name="next">Parameterless delegate invoked after <paramref name="executable"/> completes.</param>
  /// <returns>Composed executable.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="next"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, T2> Then<T1, T2>(this IExecutable<T1, Unit> executable, Func<T2> next) {
    return executable.Then(Executable.Create(next));
  }

  /// <summary>
  /// Appends an action to an executable pipeline.
  /// </summary>
  /// <param name="executable">Executable invoked first.</param>
  /// <param name="next">Action invoked with the result of <paramref name="executable"/>.</param>
  /// <returns>Composed executable.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="next"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, Unit> Then<T1, T2>(this IExecutable<T1, T2> executable, Action<T2> next) {
    return executable.Then(Executable.Create(next));
  }

  /// <summary>
  /// Appends a parameterless action to an executable pipeline.
  /// </summary>
  /// <param name="executable">Executable invoked first.</param>
  /// <param name="next">Parameterless action invoked after <paramref name="executable"/> completes.</param>
  /// <returns>Composed executable.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="next"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T, Unit> Then<T>(this IExecutable<T, Unit> executable, Action next) {
    return executable.Then(Executable.Create(next));
  }

  /// <summary>
  /// Appends an asynchronous executable to a synchronous executable.
  /// </summary>
  /// <param name="first">Synchronous executable invoked first.</param>
  /// <param name="second">Asynchronous executable invoked with the result of <paramref name="first"/>.</param>
  /// <returns>Composed asynchronous executable.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> Then<T1, T2, T3>(this IExecutable<T1, T2> first, IAsyncExecutable<T2, T3> second) {
    return first.ToAsyncExecutable().Then(second);
  }

  /// <summary>
  /// Appends an asynchronous delegate to a synchronous executable.
  /// </summary>
  /// <param name="executable">Executable invoked first.</param>
  /// <param name="next">Asynchronous delegate invoked with the result of <paramref name="executable"/>.</param>
  /// <returns>Composed asynchronous executable.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="next"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> Then<T1, T2, T3>(this IExecutable<T1, T2> executable, AsyncFunc<T2, T3> next) {
    return executable.ToAsyncExecutable().Then(AsyncExecutable.Create(next));
  }

  /// <summary>
  /// Appends a parameterless asynchronous delegate to a synchronous executable.
  /// </summary>
  /// <param name="executable">Executable invoked first.</param>
  /// <param name="next">Parameterless asynchronous delegate invoked after <paramref name="executable"/> completes.</param>
  /// <returns>Composed asynchronous executable.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="next"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T2> Then<T1, T2>(this IExecutable<T1, Unit> executable, AsyncFunc<T2> next) {
    return executable.ToAsyncExecutable().Then(AsyncExecutable.Create(next));
  }

  /// <summary>
  /// Appends an asynchronous action to a synchronous executable.
  /// </summary>
  /// <param name="executable">Executable invoked first.</param>
  /// <param name="next">Asynchronous action invoked with the result of <paramref name="executable"/>.</param>
  /// <returns>Composed asynchronous executable.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="next"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, Unit> Then<T1, T2>(this IExecutable<T1, T2> executable, AsyncAction<T2> next) {
    return executable.ToAsyncExecutable().Then(AsyncExecutable.Create(next));
  }

  /// <summary>
  /// Appends a parameterless asynchronous action to a synchronous executable.
  /// </summary>
  /// <param name="executable">Executable invoked first.</param>
  /// <param name="next">Parameterless asynchronous action invoked after <paramref name="executable"/> completes.</param>
  /// <returns>Composed asynchronous executable.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="next"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T, Unit> Then<T>(this IExecutable<T, Unit> executable, AsyncAction next) {
    return executable.ToAsyncExecutable().Then(AsyncExecutable.Create(next));
  }

  /// <summary>
  /// Branches execution into two executables and returns both results.
  /// </summary>
  /// <param name="executable">Executable that produces the shared branch input.</param>
  /// <param name="firstBranch">First branch executable.</param>
  /// <param name="secondBranch">Second branch executable.</param>
  /// <returns>Executable that returns both branch results.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="firstBranch"/> or <paramref name="secondBranch"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, (T3, T4)> Fork<T1, T2, T3, T4>(
    this IExecutable<T1, T2> executable,
    IExecutable<T2, T3> firstBranch,
    IExecutable<T2, T4> secondBranch) {
    ExceptionsHelper.ThrowIfNull(firstBranch, nameof(firstBranch));
    ExceptionsHelper.ThrowIfNull(secondBranch, nameof(secondBranch));
    return executable.Then(new ForkExecutable<T2, T3, T4>(firstBranch, secondBranch));
  }

  /// <summary>
  /// Branches execution into two delegates and returns both results.
  /// </summary>
  /// <param name="executable">Executable that produces the shared branch input.</param>
  /// <param name="firstBranch">First branch delegate.</param>
  /// <param name="secondBranch">Second branch delegate.</param>
  /// <returns>Executable that returns both branch results.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="firstBranch"/> or <paramref name="secondBranch"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, (T3, T4)> Fork<T1, T2, T3, T4>(this IExecutable<T1, T2> executable, Func<T2, T3> firstBranch, Func<T2, T4> secondBranch) {
    return executable.Fork(Executable.Create(firstBranch), Executable.Create(secondBranch));
  }

  /// <summary>
  /// Swaps items in a tuple result.
  /// </summary>
  /// <param name="fork">Executable that returns a tuple.</param>
  /// <returns>Executable that returns the tuple with reversed item order.</returns>
  [Pure]
  public static IExecutable<T1, (T3, T2)> Swap<T1, T2, T3>(this IExecutable<T1, (T2, T3)> fork) {
    return fork.Then(x => (x.Item2, x.Item1));
  }

  /// <summary>
  /// Maps the first item of a tuple result.
  /// </summary>
  /// <param name="fork">Executable that returns a tuple.</param>
  /// <param name="map">Executable applied to the first tuple item.</param>
  /// <returns>Executable with transformed first tuple item.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="map"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, (TNew, T3)> First<T1, T2, T3, TNew>(this IExecutable<T1, (T2, T3)> fork, IExecutable<T2, TNew> map) {
    ExceptionsHelper.ThrowIfNull(map, nameof(map));
    return fork.Then(new FirstMapExecutable<T2, T3, TNew>(map));
  }

  /// <summary>
  /// Maps the first item of a tuple result with a delegate.
  /// </summary>
  /// <param name="fork">Executable that returns a tuple.</param>
  /// <param name="map">Delegate applied to the first tuple item.</param>
  /// <returns>Executable with transformed first tuple item.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="map"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, (TNew, T3)> First<T1, T2, T3, TNew>(this IExecutable<T1, (T2, T3)> fork, Func<T2, TNew> map) {
    return fork.First(Executable.Create(map));
  }

  /// <summary>
  /// Maps the second item of a tuple result.
  /// </summary>
  /// <param name="fork">Executable that returns a tuple.</param>
  /// <param name="map">Executable applied to the second tuple item.</param>
  /// <returns>Executable with transformed second tuple item.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="map"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, (T2, TNew)> Second<T1, T2, T3, TNew>(this IExecutable<T1, (T2, T3)> fork, IExecutable<T3, TNew> map) {
    ExceptionsHelper.ThrowIfNull(map, nameof(map));
    return fork.Then(new SecondMapExecutable<T2, T3, TNew>(map));
  }

  /// <summary>
  /// Maps the second item of a tuple result with a delegate.
  /// </summary>
  /// <param name="fork">Executable that returns a tuple.</param>
  /// <param name="map">Delegate applied to the second tuple item.</param>
  /// <returns>Executable with transformed second tuple item.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="map"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, (T2, TNew)> Second<T1, T2, T3, TNew>(this IExecutable<T1, (T2, T3)> fork, Func<T3, TNew> map) {
    return fork.Second(Executable.Create(map));
  }

  /// <summary>
  /// Merges a tuple result with an executable.
  /// </summary>
  /// <param name="fork">Executable that returns a tuple.</param>
  /// <param name="merge">Executable that combines tuple items into a single result.</param>
  /// <returns>Executable that returns the merged result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="merge"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, T4> Merge<T1, T2, T3, T4>(this IExecutable<T1, (T2, T3)> fork, IExecutable<(T2, T3), T4> merge) {
    return fork.Then(merge);
  }

  /// <summary>
  /// Merges a tuple result with a delegate.
  /// </summary>
  /// <param name="fork">Executable that returns a tuple.</param>
  /// <param name="merge">Delegate that combines tuple items into a single result.</param>
  /// <returns>Executable that returns the merged result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="merge"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, T4> Merge<T1, T2, T3, T4>(this IExecutable<T1, (T2, T3)> fork, Func<T2, T3, T4> merge) {
    ExceptionsHelper.ThrowIfNull(merge, nameof(merge));
    return fork.Then(x => merge(x.Item1, x.Item2));
  }

  /// <summary>
  /// Adapts an executable to different external input and output types by applying mappings around it.
  /// </summary>
  /// <param name="executable">Executable being adapted.</param>
  /// <param name="incoming">Executable that converts external input to the executable input type.</param>
  /// <param name="outgoing">Executable that converts executable output to the external output type.</param>
  /// <returns>Executable with adapted input and output contracts.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="incoming"/> or <paramref name="outgoing"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, T4> Map<T1, T2, T3, T4>(this IExecutable<T2, T3> executable, IExecutable<T1, T2> incoming, IExecutable<T3, T4> outgoing) {
    return executable.Apply(ExecutionOperator.Map(incoming, outgoing));
  }

  /// <summary>
  /// Adapts an executable to different external input and output types by applying delegate mappings around it.
  /// </summary>
  /// <param name="executable">Executable being adapted.</param>
  /// <param name="incoming">Delegate that converts external input to the executable input type.</param>
  /// <param name="outgoing">Delegate that converts executable output to the external output type.</param>
  /// <returns>Executable with adapted input and output contracts.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="incoming"/> or <paramref name="outgoing"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, T4> Map<T1, T2, T3, T4>(this IExecutable<T2, T3> executable, Func<T1, T2> incoming, Func<T3, T4> outgoing) {
    return executable.Map(Executable.Create(incoming), Executable.Create(outgoing));
  }

  /// <summary>
  /// Adapts only the input type of executable.
  /// </summary>
  /// <param name="executable">Executable being adapted.</param>
  /// <param name="incoming">Executable that converts external input to the executable input type.</param>
  /// <returns>Executable with adapted input type.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="incoming"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, T3> InMap<T1, T2, T3>(this IExecutable<T2, T3> executable, IExecutable<T1, T2> incoming) {
    return executable.Map(incoming, Executable.Identity<T3>());
  }

  /// <summary>
  /// Adapts only the output type of executable.
  /// </summary>
  /// <param name="executable">Executable being adapted.</param>
  /// <param name="outgoing">Executable that converts executable output to the external output type.</param>
  /// <returns>Executable with adapted output type.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="outgoing"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, T3> OutMap<T1, T2, T3>(this IExecutable<T1, T2> executable, IExecutable<T2, T3> outgoing) {
    return executable.Map(Executable.Identity<T1>(), outgoing);
  }

  /// <summary>
  /// Adapts only the input type of executable with a delegate.
  /// </summary>
  /// <param name="executable">Executable being adapted.</param>
  /// <param name="incoming">Delegate that converts external input to the executable input type.</param>
  /// <returns>Executable with adapted input type.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="incoming"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, T3> InMap<T1, T2, T3>(this IExecutable<T2, T3> executable, Func<T1, T2> incoming) {
    return executable.InMap(Executable.Create(incoming));
  }

  /// <summary>
  /// Adapts only the output type of executable with a delegate.
  /// </summary>
  /// <param name="executable">Executable being adapted.</param>
  /// <param name="outgoing">Delegate that converts executable output to the external output type.</param>
  /// <returns>Executable with adapted output type.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="outgoing"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, T3> OutMap<T1, T2, T3>(this IExecutable<T1, T2> executable, Func<T2, T3> outgoing) {
    return executable.OutMap(Executable.Create(outgoing));
  }

  /// <summary>
  /// Runs a side effect on each result while returning the original result unchanged.
  /// </summary>
  /// <param name="executable">Executable producing the observed result.</param>
  /// <param name="action">Side-effect action invoked with the produced result.</param>
  /// <returns>Executable that preserves the original result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, T2> Tap<T1, T2>(this IExecutable<T1, T2> executable, Action<T2> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return executable.Then(new TransitiveExecutable<T2>(action));
  }

  /// <summary>
  /// Runs an asynchronous side effect on each result while returning the original result unchanged.
  /// </summary>
  /// <param name="executable">Executable producing the observed result.</param>
  /// <param name="action">Asynchronous side-effect action invoked with the produced result.</param>
  /// <returns>Asynchronous executable that preserves the original result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T2> Tap<T1, T2>(this IExecutable<T1, T2> executable, AsyncAction<T2> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return executable.Then(new AsyncTransitiveExecutable<T2>(action));
  }

  /// <summary>
  /// Executes an executable within a newly initialized interaction context.
  /// </summary>
  /// <param name="executable">Executable to run inside the context.</param>
  /// <param name="init">Context initialization logic.</param>
  /// <returns>Executable wrapped with contextual execution.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="init"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, T2> WithContext<T1, T2>(this IExecutable<T1, T2> executable, ContextInit init) {
    return executable.Apply(ExecutionOperator.Context<T1, T2>(init));
  }

  /// <summary>
  /// Builds and applies a policy pipeline to an executable.
  /// </summary>
  /// <param name="executable">Executable to wrap with policies.</param>
  /// <param name="building">Builder action that configures policies.</param>
  /// <returns>Executable wrapped with configured policies.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="building"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, T2> WithPolicy<T1, T2>(this IExecutable<T1, T2> executable, Action<PolicyBuilder<T1, T2>> building) {
    executable.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(building, nameof(building));
    var builder = new PolicyBuilder<T1, T2>();
    building(builder);
    return builder.Apply(executable);
  }

  /// <summary>
  /// Wraps executable result into <see cref="Result{T}" /> capturing thrown exceptions.
  /// </summary>
  /// <param name="executable">Executable to wrap.</param>
  /// <returns>Executable returning success or failure result.</returns>
  [Pure]
  public static IExecutable<T1, Result<T2>> WithResult<T1, T2>(this IExecutable<T1, T2> executable) {
    return executable.Apply(ResultOperator<T1, T2>.Instance);
  }

  /// <summary>
  /// Wraps an executable already returning <see cref="Result{T}"/> so that thrown exceptions are also converted to <see cref="Result{T}"/>.
  /// </summary>
  /// <param name="executable">Executable returning <see cref="Result{T}"/>.</param>
  /// <returns>Executable that preserves returned results and converts thrown exceptions to failure results.</returns>
  [Pure]
  public static IExecutable<T1, Result<T2>> WithResult<T1, T2>(this IExecutable<T1, Result<T2>> executable) {
    return executable.Apply(ResultFlattenOperator<T1, T2>.Instance);
  }

  /// <summary>
  /// Starts configuration of exception suppression for executable results.
  /// </summary>
  /// <param name="executable">Executable to configure.</param>
  /// <returns>Provider for selecting exception types to suppress.</returns>
  [Pure]
  public static SuppressExceptionOperatorProvider<T1, T2> SuppressException<T1, T2>(this IExecutable<T1, T2> executable) {
    executable.ThrowIfNullReference();
    return new SuppressExceptionOperatorProvider<T1, T2>(executable);
  }

  /// <summary>
  /// Starts configuration of exception suppression for executables returning <see cref="Optional{T}"/>.
  /// </summary>
  /// <param name="executable">Executable to configure.</param>
  /// <returns>Provider for selecting exception types to suppress.</returns>
  [Pure]
  public static SuppressExceptionFlattenOperatorProvider<T1, T2> SuppressException<T1, T2>(this IExecutable<T1, Optional<T2>> executable) {
    executable.ThrowIfNullReference();
    return new SuppressExceptionFlattenOperatorProvider<T1, T2>(executable);
  }

  /// <summary>
  /// Pipes an executable through a transformation function.
  /// </summary>
  /// <param name="executable">Source executable.</param>
  /// <param name="pipe">Transformation function.</param>
  /// <returns>Transformed executable.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="pipe"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, T3> Pipe<T1, T2, T3>(this IExecutable<T1, T2> executable, Func<IExecutable<T1, T2>, IExecutable<T1, T3>> pipe) {
    executable.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(pipe, nameof(pipe));
    return pipe(executable);
  }

  /// <summary>
  /// Pipes an executable through a transformation function producing an asynchronous executable.
  /// </summary>
  /// <param name="executable">Source executable.</param>
  /// <param name="pipe">Transformation function.</param>
  /// <returns>Transformed asynchronous executable.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="pipe"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> Pipe<T1, T2, T3>(this IExecutable<T1, T2> executable, Func<IExecutable<T1, T2>, IAsyncExecutable<T1, T3>> pipe) {
    executable.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(pipe, nameof(pipe));
    return pipe(executable);
  }

  /// <summary>
  /// Adds cache behavior to an executable.
  /// </summary>
  /// <param name="executable">Source executable.</param>
  /// <param name="storage">Cache storage used to resolve and persist values.</param>
  /// <returns>Executable with caching behavior.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="storage"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, T2> Cache<T1, T2>(this IExecutable<T1, T2> executable, ICacheStorage<T1, T2> storage) {
    return executable.Apply(ExecutionOperator.Cache(storage));
  }

  /// <summary>
  /// Adds metrics behavior to an executable.
  /// </summary>
  /// <param name="executable">Source executable.</param>
  /// <param name="metrics">Metrics sink used to record execution information.</param>
  /// <param name="tag">Optional tag passed to all metrics callbacks.</param>
  /// <returns>Executable with metrics behavior.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="metrics"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, T2> Metrics<T1, T2>(this IExecutable<T1, T2> executable, IMetrics<T1, T2> metrics, string tag = null) {
    return executable.Apply(ExecutionOperator.Metrics(metrics, tag));
  }

  /// <summary>
  /// Maps exceptions of a specific type thrown by an executable.
  /// </summary>
  /// <param name="executable">Source executable.</param>
  /// <param name="map">Function that maps the caught exception to a new exception.</param>
  /// <returns>Executable with exception mapping behavior.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="map"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, T2> MapException<T1, T2, TFrom>(this IExecutable<T1, T2> executable, Func<TFrom, Exception> map) where TFrom : Exception {
    return executable.Apply(ExecutionOperator.MapException<T1, T2, TFrom>(map));
  }

  /// <summary>
  /// Converts an executable to a handler.
  /// </summary>
  /// <returns>Handler wrapping the executable.</returns>
  [Pure]
  public static Handler<T1, T2> AsHandler<T1, T2>(this IExecutable<T1, T2> executable) {
    executable.ThrowIfNullReference();
    return new ExecutableHandler<T1, T2>(executable);
  }

  /// <summary>
  /// Returns the same handler instance.
  /// </summary>
  /// <returns>The original handler.</returns>
  [Pure]
  public static Handler<T1, T2> AsHandler<T1, T2>(this Handler<T1, T2> handler) {
    handler.ThrowIfNullReference();
    return handler;
  }

  /// <summary>
  /// Converts an executable to a query.
  /// </summary>
  /// <returns>Query wrapping the executable.</returns>
  [Pure]
  public static IQuery<T1, T2> AsQuery<T1, T2>(this IExecutable<T1, T2> executable) {
    executable.ThrowIfNullReference();
    return new ExecutableQuery<T1, T2>(executable);
  }

  /// <summary>
  /// Returns the same query instance.
  /// </summary>
  /// <returns>The original query.</returns>
  [Pure]
  public static IQuery<T1, T2> AsQuery<T1, T2>(this IQuery<T1, T2> query) {
    query.ThrowIfNullReference();
    return query;
  }

  /// <summary>
  /// Converts an executable to a command.
  /// </summary>
  /// <returns>Command wrapping the executable.</returns>
  [Pure]
  public static ICommand<T> AsCommand<T>(this IExecutable<T, bool> executable) {
    executable.ThrowIfNullReference();
    return new ExecutableCommand<T>(executable);
  }

  /// <summary>
  /// Returns the same command instance.
  /// </summary>
  /// <returns>The original command.</returns>
  [Pure]
  public static ICommand<T> AsCommand<T>(this ICommand<T> command) {
    command.ThrowIfNullReference();
    return command;
  }

  /// <summary>
  /// Converts an executable to a subscriber.
  /// </summary>
  /// <returns>Subscriber wrapping the executable.</returns>
  [Pure]
  public static ISubscriber<T> AsSubscriber<T>(this IExecutable<T, Unit> executable) {
    executable.ThrowIfNullReference();
    return new ExecutableSubscriber<T>(executable);
  }

  /// <summary>
  /// Returns the same subscriber instance.
  /// </summary>
  /// <returns>The original subscriber.</returns>
  [Pure]
  public static ISubscriber<T> AsSubscriber<T>(this ISubscriber<T> subscriber) {
    subscriber.ThrowIfNullReference();
    return subscriber;
  }

  /// <summary>
  /// Schedules executable calls on the thread pool.
  /// </summary>
  /// <returns>Thread-pool scheduled executable.</returns>
  [Pure]
  public static IExecutable<T, Unit> OnThreadPool<T>(this IExecutable<T, Unit> executable) {
    return executable.Apply(ThreadPoolOperator<T>.Instance);
  }

  /// <summary>
  /// Throttles repeated executions within the specified interval.
  /// </summary>
  /// <param name="executable">Source executable.</param>
  /// <param name="interval">Minimum interval between forwarded executions.</param>
  /// <returns>Executable with throttling behavior.</returns>
  /// <exception cref="ArgumentException"><paramref name="interval"/> is less than or equal to zero.</exception>
  [Pure]
  public static IExecutable<T, Unit> Throttle<T>(this IExecutable<T, Unit> executable, TimeSpan interval) {
    return executable.Apply(ExecutionOperator.Throttle<T>(interval));
  }

}