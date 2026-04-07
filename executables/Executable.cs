using System.Diagnostics.Contracts;

namespace Executables;

/// <summary>
/// Represents an executable operation.
/// </summary>
/// <remarks>
/// <para>
/// This interface is the composition layer of the library. It describes what should be executed,
/// without attaching runtime concerns such as exception mapping, result wrapping, caching, or metrics.
/// </para>
/// <para>
/// Create executables with <see cref="Executable.Create{T1, T2}(Func{T1, T2})">Create(...)</see> and compose them with
/// <see cref="CompositeExecutableExtensions.Then{T1, T2, T3}(IExecutable{T1, T2}, IExecutable{T2, T3})">Then(...)</see> and
/// <see cref="CompositeExecutableExtensions.Compose{T1, T2, T3}(IExecutable{T2, T3}, IExecutable{T1, T2})">Compose(...)</see>.
/// Convert to an <see cref="IExecutor{TIn,TOut}">executor</see> by calling <see cref="IExecutable{TIn,TOut}.GetExecutor">GetExecutor()</see> when runtime behavior is needed.
/// </para>
/// </remarks>
/// <example>
/// <code language="csharp">
/// IExecutable&lt;string, string&gt; pipeline =
///   Executable.Create((string text) => int.Parse(text))
///     .Then(Executable.Create((int value) => (value * 2).ToString()));
/// </code>
/// </example>
/// <seealso cref="Executable"/>
/// <seealso cref="ExecutableExtensions"/>
/// <seealso cref="CompositeExecutableExtensions"/>
/// <typeparam name="TIn">Input type.</typeparam>
/// <typeparam name="TOut">Output type.</typeparam>
public interface IExecutable<in TIn, out TOut> {

  /// <summary>
  /// Gets an <see cref="IExecutor{TIn,TOut}">executor</see> that performs this executable operation.
  /// </summary>
  /// <returns>Executor instance.</returns>
  [Pure]
  IExecutor<TIn, TOut> GetExecutor();

}