using System.Diagnostics.Contracts;
using Executables.Analytics;
using Executables.Context;
using Executables.Core.Executables;
using Executables.Core.Operators;
using Executables.Handling;
using Executables.Internal;
using Executables.Operations;
using Executables.Policies;

namespace Executables;

public static class AsyncExecutableExtensions {

  /// <summary>
  /// Returns the same asynchronous executable instance.
  /// </summary>
  [Pure]
  public static IAsyncExecutable<T1, T2> AsExecutable<T1, T2>(this IAsyncExecutable<T1, T2> executable) {
    executable.ThrowIfNullReference();
    return executable;
  }

  /// <summary>
  /// Chains an asynchronous executable with another executable-producing asynchronous executable and flattens the nested result.
  /// </summary>
  /// <param name="first">Executable invoked first.</param>
  /// <param name="second">Executable that receives the result of <paramref name="first"/> and returns the next executable to run.</param>
  /// <returns>Asynchronous executable that executes both stages as a single pipeline.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> FlatMap<T1, T2, T3>(this IAsyncExecutable<T1, T2> first, IAsyncExecutable<T2, IAsyncExecutable<T2, T3>> second) {
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return first.Then(new AsyncFlattenExecutable<T2, T3>(second));
  }

  /// <summary>
  /// Chains an asynchronous executable with an asynchronous delegate that selects the next executable to run and flattens the nested result.
  /// </summary>
  /// <param name="first">Executable invoked first.</param>
  /// <param name="second">Delegate that receives the result of <paramref name="first"/> and returns the next executable to run.</param>
  /// <returns>Asynchronous executable that executes both stages as a single pipeline.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> FlatMap<T1, T2, T3>(this IAsyncExecutable<T1, T2> first, AsyncFunc<T2, IAsyncExecutable<T2, T3>> second) {
    return first.Then(AsyncExecutable.FlatMap(second));
  }

  /// <summary>
  /// Chains an asynchronous executable with a delegate that selects the next executable to run and flattens the nested result.
  /// </summary>
  /// <param name="first">Executable invoked first.</param>
  /// <param name="second">Delegate that receives the result of <paramref name="first"/> and returns the next executable to run.</param>
  /// <returns>Asynchronous executable that executes both stages as a single pipeline.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> FlatMap<T1, T2, T3>(this IAsyncExecutable<T1, T2> first, Func<T2, IAsyncExecutable<T2, T3>> second) {
    return first.Then(AsyncExecutable.FlatMap(second));
  }

  /// <summary>
  /// Appends an asynchronous projection while preserving the previous result in the returned tuple.
  /// </summary>
  /// <param name="first">Executable that produces the value passed to <paramref name="second"/>.</param>
  /// <param name="second">Asynchronous delegate that computes an additional value from the result of <paramref name="first"/>.</param>
  /// <returns>Asynchronous executable that returns both the original result and the appended value.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, (T2, T3)> Accumulate<T1, T2, T3>(this IAsyncExecutable<T1, T2> first, AsyncFunc<T2, T3> second) {
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return first.Then(AsyncExecutable.Create(async (T2 t2, CancellationToken token) => {
      T3 t3 = await second(t2, token);
      return (t2, t3);
    }));
  }

  /// <summary>
  /// Appends an asynchronous projection while preserving the accumulated tuple produced by the previous stage.
  /// </summary>
  /// <param name="first">Executable that produces the tuple passed to <paramref name="second"/>.</param>
  /// <param name="second">Asynchronous delegate that computes an additional value from the accumulated tuple items.</param>
  /// <returns>Asynchronous executable that returns the original tuple items plus the appended value.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, (T2, T3, T4)> Accumulate<T1, T2, T3, T4>(this IAsyncExecutable<T1, (T2, T3)> first, AsyncFunc<T2, T3, T4> second) {
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return first.Then(AsyncExecutable.Create(async (T2 t2, T3 t3, CancellationToken token) => {
      T4 t4 = await second(t2, t3, token);
      return (t2, t3, t4);
    }));
  }

  /// <summary>
  /// Appends an asynchronous projection while preserving the accumulated tuple produced by the previous stage.
  /// </summary>
  /// <param name="first">Executable that produces the tuple passed to <paramref name="second"/>.</param>
  /// <param name="second">Asynchronous delegate that computes an additional value from the accumulated tuple items.</param>
  /// <returns>Asynchronous executable that returns the original tuple items plus the appended value.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, (T2, T3, T4, T5)> Accumulate<T1, T2, T3, T4, T5>(
    this IAsyncExecutable<T1, (T2, T3, T4)> first,
    AsyncFunc<T2, T3, T4, T5> second) {
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return first.Then(AsyncExecutable.Create(async (T2 t2, T3 t3, T4 t4, CancellationToken token) => {
      T5 t5 = await second(t2, t3, t4, token);
      return (t2, t3, t4, t5);
    }));
  }

  /// <summary>
  /// Branches asynchronous execution into two asynchronous executables and returns both results.
  /// </summary>
  /// <param name="executable">Executable that produces the shared branch input.</param>
  /// <param name="firstBranch">First branch executable.</param>
  /// <param name="secondBranch">Second branch executable.</param>
  /// <returns>Executable that returns both branch results.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="firstBranch"/> or <paramref name="secondBranch"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, (T3, T4)> Fork<T1, T2, T3, T4>(
    this IAsyncExecutable<T1, T2> executable,
    IAsyncExecutable<T2, T3> firstBranch,
    IAsyncExecutable<T2, T4> secondBranch) {
    ExceptionsHelper.ThrowIfNull(firstBranch, nameof(firstBranch));
    ExceptionsHelper.ThrowIfNull(secondBranch, nameof(secondBranch));
    return executable.Then(new AsyncForkExecutable<T2, T3, T4>(firstBranch, secondBranch));
  }

  /// <summary>
  /// Branches asynchronous execution into two asynchronous delegates and returns both results.
  /// </summary>
  /// <param name="executable">Executable that produces the shared branch input.</param>
  /// <param name="firstBranch">First branch delegate.</param>
  /// <param name="secondBranch">Second branch delegate.</param>
  /// <returns>Executable that returns both branch results.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="firstBranch"/> or <paramref name="secondBranch"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, (T3, T4)> Fork<T1, T2, T3, T4>(
    this IAsyncExecutable<T1, T2> executable,
    AsyncFunc<T2, T3> firstBranch,
    AsyncFunc<T2, T4> secondBranch) {
    return executable.Fork(AsyncExecutable.Create(firstBranch), AsyncExecutable.Create(secondBranch));
  }

  /// <summary>
  /// Swaps items in a tuple result.
  /// </summary>
  /// <param name="fork">Executable that returns a tuple.</param>
  /// <returns>Executable that returns the tuple with reversed item order.</returns>
  [Pure]
  public static IAsyncExecutable<T1, (T3, T2)> Swap<T1, T2, T3>(this IAsyncExecutable<T1, (T2, T3)> fork) {
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
  public static IAsyncExecutable<T1, (TNew, T3)> First<T1, T2, T3, TNew>(this IAsyncExecutable<T1, (T2, T3)> fork, IAsyncExecutable<T2, TNew> map) {
    ExceptionsHelper.ThrowIfNull(map, nameof(map));
    return fork.Then(new AsyncFirstMapExecutable<T2, T3, TNew>(map));
  }

  /// <summary>
  /// Maps the first item of a tuple result with an asynchronous delegate.
  /// </summary>
  /// <param name="fork">Executable that returns a tuple.</param>
  /// <param name="map">Delegate applied to the first tuple item.</param>
  /// <returns>Executable with transformed first tuple item.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="map"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, (TNew, T3)> First<T1, T2, T3, TNew>(this IAsyncExecutable<T1, (T2, T3)> fork, AsyncFunc<T2, TNew> map) {
    return fork.First(AsyncExecutable.Create(map));
  }

  /// <summary>
  /// Maps the second item of a tuple result.
  /// </summary>
  /// <param name="fork">Executable that returns a tuple.</param>
  /// <param name="map">Executable applied to the second tuple item.</param>
  /// <returns>Executable with transformed second tuple item.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="map"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, (T2, TNew)> Second<T1, T2, T3, TNew>(this IAsyncExecutable<T1, (T2, T3)> fork, IAsyncExecutable<T3, TNew> map) {
    ExceptionsHelper.ThrowIfNull(map, nameof(map));
    return fork.Then(new AsyncSecondMapExecutable<T2, T3, TNew>(map));
  }

  /// <summary>
  /// Maps the second item of a tuple result with an asynchronous delegate.
  /// </summary>
  /// <param name="fork">Executable that returns a tuple.</param>
  /// <param name="map">Delegate applied to the second tuple item.</param>
  /// <returns>Executable with transformed second tuple item.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="map"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, (T2, TNew)> Second<T1, T2, T3, TNew>(this IAsyncExecutable<T1, (T2, T3)> fork, AsyncFunc<T3, TNew> map) {
    return fork.Second(AsyncExecutable.Create(map));
  }

  /// <summary>
  /// Merges a tuple result with a delegate.
  /// </summary>
  /// <param name="fork">Executable that returns a tuple.</param>
  /// <param name="merge">Delegate that combines tuple items into a single result.</param>
  /// <returns>Executable that returns the merged result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="merge"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T4> Merge<T1, T2, T3, T4>(this IAsyncExecutable<T1, (T2, T3)> fork, Func<T2, T3, T4> merge) {
    ExceptionsHelper.ThrowIfNull(merge, nameof(merge));
    return fork.Then(x => merge(x.Item1, x.Item2));
  }

  /// <summary>
  /// Merges a three-item tuple result with a delegate.
  /// </summary>
  /// <param name="executable">Executable that returns a three-item tuple.</param>
  /// <param name="merge">Delegate that combines tuple items into a single result.</param>
  /// <returns>Asynchronous executable that returns the merged result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="merge"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T5> Merge<T1, T2, T3, T4, T5>(this IAsyncExecutable<T1, (T2, T3, T4)> executable, Func<T2, T3, T4, T5> merge) {
    ExceptionsHelper.ThrowIfNull(merge, nameof(merge));
    return executable.Then(x => merge(x.Item1, x.Item2, x.Item3));
  }

  /// <summary>
  /// Merges a four-item tuple result with a delegate.
  /// </summary>
  /// <param name="executable">Executable that returns a four-item tuple.</param>
  /// <param name="merge">Delegate that combines tuple items into a single result.</param>
  /// <returns>Asynchronous executable that returns the merged result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="merge"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T6> Merge<T1, T2, T3, T4, T5, T6>(this IAsyncExecutable<T1, (T2, T3, T4, T5)> executable, Func<T2, T3, T4, T5, T6> merge) {
    ExceptionsHelper.ThrowIfNull(merge, nameof(merge));
    return executable.Then(x => merge(x.Item1, x.Item2, x.Item3, x.Item4));
  }

  /// <summary>
  /// Adapts an asynchronous executable to different external input and output types by applying delegate mappings around it.
  /// </summary>
  /// <param name="executable">Asynchronous executable being adapted.</param>
  /// <param name="incoming">Delegate that converts external input to the executable input type.</param>
  /// <param name="outgoing">Delegate that converts executable output to the external output type.</param>
  /// <returns>Asynchronous executable with adapted input and output contracts.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="incoming"/> or <paramref name="outgoing"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T4> Map<T1, T2, T3, T4>(this IAsyncExecutable<T2, T3> executable, Func<T1, T2> incoming, Func<T3, T4> outgoing) {
    return executable.Compose(incoming).Then(outgoing);
  }

  /// <summary>
  /// Runs a synchronous side effect on each asynchronous result while returning the original result unchanged.
  /// </summary>
  /// <param name="executable">Executable producing the observed result.</param>
  /// <param name="action">Side-effect action invoked with the produced result.</param>
  /// <returns>Asynchronous executable that preserves the original result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T2> Tap<T1, T2>(this IAsyncExecutable<T1, T2> executable, Action<T2> action) {
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
  public static IAsyncExecutable<T1, T2> Tap<T1, T2>(this IAsyncExecutable<T1, T2> executable, AsyncAction<T2> action) {
    ExceptionsHelper.ThrowIfNull(action, nameof(action));
    return executable.Then(new AsyncTransitiveExecutable<T2>(action));
  }

  /// <summary>
  /// Executes an asynchronous executable within a newly initialized interaction context.
  /// </summary>
  /// <param name="executable">Executable to run inside the context.</param>
  /// <param name="init">Context initialization logic.</param>
  /// <returns>Executable wrapped with contextual execution.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="init"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T2> WithContext<T1, T2>(this IAsyncExecutable<T1, T2> executable, ContextInit init) {
    return executable.Apply(AsyncExecutionOperator.Context<T1, T2>(init));
  }

  /// <summary>
  /// Builds and applies an asynchronous policy pipeline to an executable.
  /// </summary>
  /// <param name="executable">Executable to wrap with policies.</param>
  /// <param name="building">Builder action that configures policies.</param>
  /// <returns>Executable wrapped with configured policies.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="building"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T2> WithPolicy<T1, T2>(this IAsyncExecutable<T1, T2> executable, Action<AsyncPolicyBuilder<T1, T2>> building) {
    executable.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(building, nameof(building));
    var source = new AsyncPolicyBuilder<T1, T2>();
    building(source);
    return source.Apply(executable);
  }

  /// <summary>
  /// Wraps executable result into <see cref="Result{T}" /> capturing thrown exceptions.
  /// </summary>
  /// <param name="executable">Executable to wrap.</param>
  /// <returns>Executable returning success or failure result.</returns>
  [Pure]
  public static IAsyncExecutable<T1, Result<T2>> WithResult<T1, T2>(this IAsyncExecutable<T1, T2> executable) {
    return executable.Apply(AsyncResultOperator<T1, T2>.Instance);
  }

  /// <summary>
  /// Wraps an executable already returning <see cref="Result{T}"/> so that thrown exceptions are also converted to <see cref="Result{T}"/>.
  /// </summary>
  /// <param name="executable">Executable returning <see cref="Result{T}"/>.</param>
  /// <returns>Executable that preserves returned results and converts thrown exceptions to failure results.</returns>
  [Pure]
  public static IAsyncExecutable<T1, Result<T2>> WithResult<T1, T2>(this IAsyncExecutable<T1, Result<T2>> executable) {
    return executable.Apply(AsyncResultFlattenOperator<T1, T2>.Instance);
  }

  /// <summary>
  /// Starts configuration of exception suppression for asynchronous executable results.
  /// </summary>
  /// <param name="executable">Executable to configure.</param>
  /// <returns>Provider for selecting exception types to suppress.</returns>
  [Pure]
  public static SuppressExceptionAsyncOperatorProvider<T1, T2> SuppressException<T1, T2>(this IAsyncExecutable<T1, T2> executable) {
    executable.ThrowIfNullReference();
    return new SuppressExceptionAsyncOperatorProvider<T1, T2>(executable);
  }

  /// <summary>
  /// Starts configuration of exception suppression for asynchronous executables returning <see cref="Optional{T}" />.
  /// </summary>
  /// <param name="executable">Executable to configure.</param>
  /// <returns>Provider for selecting exception types to suppress.</returns>
  [Pure]
  public static SuppressExceptionAsyncFlattenOperatorProvider<T1, T2> SuppressException<T1, T2>(this IAsyncExecutable<T1, Optional<T2>> executable) {
    executable.ThrowIfNullReference();
    return new SuppressExceptionAsyncFlattenOperatorProvider<T1, T2>(executable);
  }

  /// <summary>
  /// Pipes an asynchronous executable through a transformation function.
  /// </summary>
  /// <param name="executable">Source executable.</param>
  /// <param name="pipe">Transformation function.</param>
  /// <returns>Transformed asynchronous executable.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="pipe"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> Pipe<T1, T2, T3>(
    this IAsyncExecutable<T1, T2> executable,
    Func<IAsyncExecutable<T1, T2>, IAsyncExecutable<T1, T3>> pipe) {
    executable.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(pipe, nameof(pipe));
    return pipe(executable);
  }

  /// <summary>
  /// Adds asynchronous cache behavior to an executable.
  /// </summary>
  /// <param name="executable">Source asynchronous executable.</param>
  /// <param name="storage">Cache storage used to resolve and persist values.</param>
  /// <returns>Asynchronous executable with caching behavior.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="storage"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T2> Cache<T1, T2>(this IAsyncExecutable<T1, T2> executable, ICacheStorage<T1, T2> storage) {
    return executable.Apply(AsyncExecutionOperator.Cache(storage));
  }

  /// <summary>
  /// Adds asynchronous metrics behavior to an executable.
  /// </summary>
  /// <param name="executable">Source asynchronous executable.</param>
  /// <param name="metrics">Metrics sink used to record execution information.</param>
  /// <param name="tag">Optional tag passed to all metrics callbacks.</param>
  /// <returns>Asynchronous executable with metrics behavior.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="metrics"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T2> Metrics<T1, T2>(this IAsyncExecutable<T1, T2> executable, IMetrics<T1, T2> metrics, string tag = null) {
    return executable.Apply(AsyncExecutionOperator.Metrics(metrics, tag));
  }

  /// <summary>
  /// Maps exceptions of a specific type thrown by an asynchronous executable.
  /// </summary>
  /// <param name="executable">Source executable.</param>
  /// <param name="map">Function that maps the caught exception to a new exception.</param>
  /// <returns>Asynchronous executable with exception mapping behavior.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="map"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T2> MapException<T1, T2, TFrom>(this IAsyncExecutable<T1, T2> executable, Func<TFrom, Exception> map)
    where TFrom : Exception {
    return executable.Apply(AsyncExecutionOperator.MapException<T1, T2, TFrom>(map));
  }

  /// <summary>
  /// Converts an asynchronous executable to an asynchronous handler.
  /// </summary>
  /// <returns>Async handler wrapping the executable.</returns>
  [Pure]
  public static AsyncHandler<T1, T2> AsHandler<T1, T2>(this IAsyncExecutable<T1, T2> executable) {
    executable.ThrowIfNullReference();
    return new AsyncExecutableHandler<T1, T2>(executable);
  }

  /// <summary>
  /// Returns the same asynchronous handler instance.
  /// </summary>
  /// <returns>The original async handler.</returns>
  [Pure]
  public static AsyncHandler<T1, T2> AsHandler<T1, T2>(this AsyncHandler<T1, T2> handler) {
    handler.ThrowIfNullReference();
    return handler;
  }

  /// <summary>
  /// Converts an asynchronous executable to an asynchronous query.
  /// </summary>
  /// <returns>Async query wrapping the executable.</returns>
  [Pure]
  public static IAsyncQuery<T1, T2> AsQuery<T1, T2>(this IAsyncExecutable<T1, T2> executable) {
    executable.ThrowIfNullReference();
    return new AsyncExecutableQuery<T1, T2>(executable);
  }

  /// <summary>
  /// Returns the same asynchronous query instance.
  /// </summary>
  /// <returns>The original async query.</returns>
  [Pure]
  public static IAsyncQuery<T1, T2> AsQuery<T1, T2>(this IAsyncQuery<T1, T2> query) {
    query.ThrowIfNullReference();
    return query;
  }

  /// <summary>
  /// Converts an asynchronous executable to an asynchronous command.
  /// </summary>
  /// <returns>Async command wrapping the executable.</returns>
  [Pure]
  public static IAsyncCommand<T> AsCommand<T>(this IAsyncExecutable<T, bool> executable) {
    executable.ThrowIfNullReference();
    return new AsyncExecutableCommand<T>(executable);
  }

  /// <summary>
  /// Returns the same asynchronous command instance.
  /// </summary>
  /// <returns>The original async command.</returns>
  [Pure]
  public static IAsyncCommand<T> AsCommand<T>(this IAsyncCommand<T> command) {
    command.ThrowIfNullReference();
    return command;
  }

}