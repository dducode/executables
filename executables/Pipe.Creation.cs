using System.Diagnostics.Contracts;
using Executables.Core.Pipes;
using Executables.Internal;

namespace Executables;

/// <summary>
/// Factory methods for creating pipe instances.
/// </summary>
public static class Pipe {

  /// <summary>
  /// Returns a pipe that passes input through unchanged.
  /// </summary>
  /// <typeparam name="T">Input and output type.</typeparam>
  /// <returns>Identity pipe instance.</returns>
  [Pure]
  public static IPipe<T, T> Identity<T>() {
    return IdentityPipe<T>.Instance;
  }

  /// <summary>
  /// Creates a pipe from a transformation function.
  /// </summary>
  /// <typeparam name="T1">Input type.</typeparam>
  /// <typeparam name="T2">Output type.</typeparam>
  /// <param name="pipe">Transformation function.</param>
  /// <returns>Pipe that delegates execution to <paramref name="pipe"/>.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="pipe"/> is <see langword="null"/>.</exception>
  [Pure]
  public static IPipe<T1, T2> Create<T1, T2>(Func<T1, T2> pipe) {
    ExceptionsHelper.ThrowIfNull(pipe, nameof(pipe));
    return new AnonymousPipe<T1, T2>(pipe);
  }

}
