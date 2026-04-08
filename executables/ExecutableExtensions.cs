using System.Diagnostics.Contracts;
using Executables.Core.Executables;
using Executables.Handling;
using Executables.Internal;
using Executables.Subscribers;

namespace Executables;

/// <summary>
/// Extension methods for composing and adapting executables.
/// </summary>
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
  /// Chains an executable with another executable-producing executable and flattens the nested result.
  /// </summary>
  /// <param name="first">Executable invoked first.</param>
  /// <param name="second">Executable that receives the result of <paramref name="first"/> and returns the next executable to run.</param>
  /// <returns>Executable that executes both stages as a single pipeline.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, T3> FlatMap<T1, T2, T3>(this IExecutable<T1, T2> first, IExecutable<T2, IExecutable<T2, T3>> second) {
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return first.Then(new FlattenExecutable<T2, T3>(second));
  }

  /// <summary>
  /// Chains an executable with a delegate that selects the next executable to run and flattens the nested result.
  /// </summary>
  /// <param name="first">Executable invoked first.</param>
  /// <param name="second">Delegate that receives the result of <paramref name="first"/> and returns the next executable to run.</param>
  /// <returns>Executable that executes both stages as a single pipeline.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, T3> FlatMap<T1, T2, T3>(this IExecutable<T1, T2> first, Func<T2, IExecutable<T2, T3>> second) {
    return first.Then(Executable.FlatMap(second));
  }

  /// <summary>
  /// Appends a projection while preserving the previous result in the returned tuple.
  /// </summary>
  /// <param name="first">Executable that produces the value passed to <paramref name="second"/>.</param>
  /// <param name="second">Delegate that computes an additional value from the result of <paramref name="first"/>.</param>
  /// <returns>Executable that returns both the original result and the appended value.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, (T2, T3)> Accumulate<T1, T2, T3>(this IExecutable<T1, T2> first, Func<T2, T3> second) {
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return first.Then(Executable.Create((T2 t2) => {
      T3 t3 = second(t2);
      return (t2, t3);
    }));
  }

  /// <summary>
  /// Appends a projection while preserving the accumulated tuple produced by the previous stage.
  /// </summary>
  /// <param name="first">Executable that produces the tuple passed to <paramref name="second"/>.</param>
  /// <param name="second">Delegate that computes an additional value from the accumulated tuple items.</param>
  /// <returns>Executable that returns the original tuple items plus the appended value.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, (T2, T3, T4)> Accumulate<T1, T2, T3, T4>(this IExecutable<T1, (T2, T3)> first, Func<T2, T3, T4> second) {
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return first.Then(Executable.Create((T2 t2, T3 t3) => {
      T4 t4 = second(t2, t3);
      return (t2, t3, t4);
    }));
  }

  /// <summary>
  /// Appends a projection while preserving the accumulated tuple produced by the previous stage.
  /// </summary>
  /// <param name="first">Executable that produces the tuple passed to <paramref name="second"/>.</param>
  /// <param name="second">Delegate that computes an additional value from the accumulated tuple items.</param>
  /// <returns>Executable that returns the original tuple items plus the appended value.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, (T2, T3, T4, T5)> Accumulate<T1, T2, T3, T4, T5>(this IExecutable<T1, (T2, T3, T4)> first, Func<T2, T3, T4, T5> second) {
    ExceptionsHelper.ThrowIfNull(second, nameof(second));
    return first.Then(Executable.Create((T2 t2, T3 t3, T4 t4) => {
      T5 t5 = second(t2, t3, t4);
      return (t2, t3, t4, t5);
    }));
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
  /// Merges a three-item tuple result with a delegate.
  /// </summary>
  /// <param name="executable">Executable that returns a three-item tuple.</param>
  /// <param name="merge">Delegate that combines tuple items into a single result.</param>
  /// <returns>Executable that returns the merged result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="merge"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, T5> Merge<T1, T2, T3, T4, T5>(this IExecutable<T1, (T2, T3, T4)> executable, Func<T2, T3, T4, T5> merge) {
    ExceptionsHelper.ThrowIfNull(merge, nameof(merge));
    return executable.Then(x => merge(x.Item1, x.Item2, x.Item3));
  }

  /// <summary>
  /// Merges a four-item tuple result with a delegate.
  /// </summary>
  /// <param name="executable">Executable that returns a four-item tuple.</param>
  /// <param name="merge">Delegate that combines tuple items into a single result.</param>
  /// <returns>Executable that returns the merged result.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="merge"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IExecutable<T1, T6> Merge<T1, T2, T3, T4, T5, T6>(this IExecutable<T1, (T2, T3, T4, T5)> executable, Func<T2, T3, T4, T5, T6> merge) {
    ExceptionsHelper.ThrowIfNull(merge, nameof(merge));
    return executable.Then(x => merge(x.Item1, x.Item2, x.Item3, x.Item4));
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
    return executable.Compose(incoming).Then(outgoing);
  }

  /// <summary>
  /// Adapts an endomorphism through an isomorphism.
  /// </summary>
  /// <typeparam name="T1">External input and output type.</typeparam>
  /// <typeparam name="T2">Internal input and output type of the executable.</typeparam>
  /// <param name="executable">Executable that transforms values of type <typeparamref name="T2"/>.</param>
  /// <param name="iso">Isomorphism used to convert values into and out of <typeparamref name="T2"/>.</param>
  /// <returns>Executable that transforms values of type <typeparamref name="T1"/> by mapping through <paramref name="iso"/>.</returns>
  [Pure]
  public static IExecutable<T1, T1> MapIso<T1, T2>(this IExecutable<T2, T2> executable, IIso<T1, T2> iso) {
    return executable.Compose((T1 t1) => iso.Forward(t1)).Then(iso.Backward);
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

}