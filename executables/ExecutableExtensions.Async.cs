using System.Diagnostics.Contracts;
using Executables.Context;
using Executables.Core.Executables;
using Executables.Core.Operators;
using Executables.Handling;
using Executables.Internal;
using Executables.Operations;
using Executables.Policies;

namespace Executables;

public static partial class ExecutableExtensions {

  /// <summary>
  /// Returns the same asynchronous executable instance.
  /// </summary>
  [Pure]
  public static IAsyncExecutable<T1, T2> AsExecutable<T1, T2>(this IAsyncExecutable<T1, T2> executable) {
    executable.ThrowIfNullReference();
    return executable;
  }

  /// <summary>
  /// Composes two asynchronous executables into a single pipeline.
  /// </summary>
  /// <param name="first">Executable invoked first.</param>
  /// <param name="second">Executable invoked with the result of <paramref name="first"/>.</param>
  /// <returns>Composed asynchronous executable.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> Then<T1, T2, T3>(this IAsyncExecutable<T1, T2> first, IAsyncExecutable<T2, T3> second) {
    first.ThrowIfNullReference();
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    if (first is AsyncIdentityExecutable<T1>)
      return (IAsyncExecutable<T1, T3>)second;
    return new AsyncCompositeExecutable<T1, T2, T3>(first, second);
  }

  /// <summary>
  /// Appends an asynchronous delegate to an asynchronous executable pipeline.
  /// </summary>
  /// <param name="executable">Executable invoked first.</param>
  /// <param name="next">Asynchronous delegate invoked with the result of <paramref name="executable"/>.</param>
  /// <returns>Composed asynchronous executable.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="next"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> Then<T1, T2, T3>(this IAsyncExecutable<T1, T2> executable, AsyncFunc<T2, T3> next) {
    return executable.Then(AsyncExecutable.Create(next));
  }

  /// <summary>
  /// Appends a synchronous executable to an asynchronous executable.
  /// </summary>
  /// <param name="first">Asynchronous executable invoked first.</param>
  /// <param name="second">Synchronous executable invoked with the result of <paramref name="first"/>.</param>
  /// <returns>Composed asynchronous executable.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> Then<T1, T2, T3>(this IAsyncExecutable<T1, T2> first, IExecutable<T2, T3> second) {
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return first.Then(second.ToAsyncExecutable());
  }

  /// <summary>
  /// Appends a synchronous delegate to an asynchronous executable.
  /// </summary>
  /// <param name="executable">Executable invoked first.</param>
  /// <param name="next">Delegate invoked with the result of <paramref name="executable"/>.</param>
  /// <returns>Composed asynchronous executable.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="next"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> Then<T1, T2, T3>(this IAsyncExecutable<T1, T2> executable, Func<T2, T3> next) {
    return executable.Then(Executable.Create(next));
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
  /// Merges a tuple result with an asynchronous executable.
  /// </summary>
  /// <param name="fork">Executable that returns a tuple.</param>
  /// <param name="merge">Executable that combines tuple items into a single result.</param>
  /// <returns>Executable that returns the merged result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="merge"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T4> Merge<T1, T2, T3, T4>(this IAsyncExecutable<T1, (T2, T3)> fork, IAsyncExecutable<(T2, T3), T4> merge) {
    return fork.Then(merge);
  }

  /// <summary>
  /// Merges a tuple result with an asynchronous delegate.
  /// </summary>
  /// <param name="fork">Executable that returns a tuple.</param>
  /// <param name="merge">Delegate that combines tuple items into a single result.</param>
  /// <returns>Executable that returns the merged result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="merge"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T4> Merge<T1, T2, T3, T4>(this IAsyncExecutable<T1, (T2, T3)> fork, AsyncFunc<T2, T3, T4> merge) {
    ExceptionsHelper.ThrowIfNull(merge, nameof(merge));
    return fork.Then(async (x, t) => await merge(x.Item1, x.Item2, t));
  }

  /// <summary>
  /// Races multiple asynchronous executables and returns the first completed result.
  /// </summary>
  /// <param name="executable">Executable that produces the shared race input.</param>
  /// <param name="executables">Executables competing for the first result.</param>
  /// <returns>Executable that returns the first completed result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="executables"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentException"><paramref name="executables"/> is empty.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> Race<T1, T2, T3>(this IAsyncExecutable<T1, T2> executable, params IAsyncExecutable<T2, T3>[] executables) {
    ExceptionsHelper.ThrowIfNullOrEmpty(executables, nameof(executables));
    return executables.Length > 1 ? executable.Then(new RaceExecutable<T2, T3>(executables)) : executable.Then(executables[0]);
  }

  /// <summary>
  /// Races multiple asynchronous delegates and returns the first completed result.
  /// </summary>
  /// <param name="executable">Executable that produces the shared race input.</param>
  /// <param name="executables">Delegates competing for the first result.</param>
  /// <returns>Executable that returns the first completed result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="executables"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentException"><paramref name="executables"/> is empty.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> Race<T1, T2, T3>(this IAsyncExecutable<T1, T2> executable, params AsyncFunc<T2, T3>[] executables) {
    ExceptionsHelper.ThrowIfNullOrEmpty(executables, nameof(executables));
    return executables.Length > 1 ? executable.Race(executables.Select(AsyncExecutable.Create).ToArray()) : executable.Then(executables[0]);
  }

  /// <summary>
  /// Races multiple asynchronous executables and returns the first successful result.
  /// </summary>
  /// <param name="executable">Executable that produces the shared race input.</param>
  /// <param name="executables">Executables competing for the first successful result.</param>
  /// <returns>Executable that returns the first successful result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="executables"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentException"><paramref name="executables"/> is empty.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> RaceSuccess<T1, T2, T3>(this IAsyncExecutable<T1, T2> executable, params IAsyncExecutable<T2, T3>[] executables) {
    ExceptionsHelper.ThrowIfNullOrEmpty(executables, nameof(executables));
    return executables.Length > 1 ? executable.Then(new RaceSuccessExecutable<T2, T3>(executables)) : executable.Then(executables[0]);
  }

  /// <summary>
  /// Races multiple asynchronous delegates and returns the first successful result.
  /// </summary>
  /// <param name="executable">Executable that produces the shared race input.</param>
  /// <param name="executables">Delegates competing for the first successful result.</param>
  /// <returns>Executable that returns the first successful result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="executables"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentException"><paramref name="executables"/> is empty.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> RaceSuccess<T1, T2, T3>(this IAsyncExecutable<T1, T2> executable, params AsyncFunc<T2, T3>[] executables) {
    ExceptionsHelper.ThrowIfNullOrEmpty(executables, nameof(executables));
    return executables.Length > 1 ? executable.RaceSuccess(executables.Select(AsyncExecutable.Create).ToArray()) : executable.Then(executables[0]);
  }

  /// <summary>
  /// Adapts an asynchronous executable to different external input and output types by applying mappings around it.
  /// </summary>
  /// <param name="executable">Asynchronous executable being adapted.</param>
  /// <param name="incoming">Executable that converts external input to the executable input type.</param>
  /// <param name="outgoing">Executable that converts executable output to the external output type.</param>
  /// <returns>Asynchronous executable with adapted input and output contracts.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="incoming"/> or <paramref name="outgoing"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T4> Map<T1, T2, T3, T4>(
    this IAsyncExecutable<T2, T3> executable,
    IExecutable<T1, T2> incoming,
    IExecutable<T3, T4> outgoing) {
    return executable.Apply(AsyncExecutionOperator.Map(incoming, outgoing));
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
    return executable.Map(Executable.Create(incoming), Executable.Create(outgoing));
  }

  /// <summary>
  /// Adapts only the input type of asynchronous executable.
  /// </summary>
  /// <param name="executable">Asynchronous executable being adapted.</param>
  /// <param name="incoming">Executable that converts external input to the executable input type.</param>
  /// <returns>Executable with adapted input type.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="incoming"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> InMap<T1, T2, T3>(this IAsyncExecutable<T2, T3> executable, IExecutable<T1, T2> incoming) {
    return executable.Map(incoming, Executable.Identity<T3>());
  }

  /// <summary>
  /// Adapts only the output type of asynchronous executable.
  /// </summary>
  /// <param name="executable">Asynchronous executable being adapted.</param>
  /// <param name="outgoing">Executable that converts executable output to the external output type.</param>
  /// <returns>Executable with adapted output type.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="outgoing"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> OutMap<T1, T2, T3>(this IAsyncExecutable<T1, T2> executable, IExecutable<T2, T3> outgoing) {
    return executable.Map(Executable.Identity<T1>(), outgoing);
  }

  /// <summary>
  /// Adapts only the input type of asynchronous executable with a delegate.
  /// </summary>
  /// <param name="executable">Asynchronous executable being adapted.</param>
  /// <param name="incoming">Delegate that converts external input to the executable input type.</param>
  /// <returns>Executable with adapted input type.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="incoming"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> InMap<T1, T2, T3>(this IAsyncExecutable<T2, T3> executable, Func<T1, T2> incoming) {
    return executable.InMap(Executable.Create(incoming));
  }

  /// <summary>
  /// Adapts only the output type of asynchronous executable with a delegate.
  /// </summary>
  /// <param name="executable">Asynchronous executable being adapted.</param>
  /// <param name="outgoing">Delegate that converts executable output to the external output type.</param>
  /// <returns>Executable with adapted output type.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="outgoing"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IAsyncExecutable<T1, T3> OutMap<T1, T2, T3>(this IAsyncExecutable<T1, T2> executable, Func<T2, T3> outgoing) {
    return executable.OutMap(Executable.Create(outgoing));
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
    ExceptionsHelper.ThrowIfNull(init, nameof(init));
    return executable.Apply(new AsyncContextOperator<T1, T2>(init));
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
    return executable.Apply(new AsyncResultOperator<T1, T2>());
  }

  /// <summary>
  /// Returns the same executable when result wrapping is already applied.
  /// </summary>
  /// <param name="executable">Executable already returning <see cref="Result{T}" />.</param>
  /// <returns>The original executable.</returns>
  [Pure]
  public static IAsyncExecutable<T1, Result<T2>> WithResult<T1, T2>(this IAsyncExecutable<T1, Result<T2>> executable) {
    executable.ThrowIfNullReference();
    return executable;
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
  public static SuppressExceptionAsyncOptionalOperatorProvider<T1, T2> SuppressException<T1, T2>(this IAsyncExecutable<T1, Optional<T2>> executable) {
    executable.ThrowIfNullReference();
    return new SuppressExceptionAsyncOptionalOperatorProvider<T1, T2>(executable);
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