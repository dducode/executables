namespace Executables;

/// <summary>
/// Represents an executor that processes input and returns a result.
/// </summary>
/// <remarks>
/// <para>
/// Executor is the runtime layer of the library. It performs execution and is the place where
/// policies, operators, context initialization, error handling, caching, and metrics are applied.
/// </para>
/// <para>
/// Obtain an executor from an executable and then attach runtime behavior such as operators, result wrapping, or
/// exception mapping.
/// </para>
/// </remarks>
/// <example>
/// <code language="csharp">
/// IExecutor&lt;string, Result&lt;int&gt;&gt; executor =
///   Executable.Create((string text) => int.Parse(text))
///     .GetExecutor()
///     .MapException((FormatException ex) => new InvalidOperationException("Invalid number", ex))
///     .WithResult();
/// </code>
/// </example>
/// <seealso cref="ExecutorExtensions"/>
/// <seealso cref="Operations.OperationsExtensions">OperationsExtensions</seealso>
/// <typeparam name="TIn">Input type.</typeparam>
/// <typeparam name="TOut">Output type.</typeparam>
public interface IExecutor<in TIn, out TOut> {

  /// <summary>
  /// Executes the operation for the specified input value.
  /// </summary>
  /// <param name="input">Input value.</param>
  /// <returns>Execution result.</returns>
  TOut Execute(TIn input);

}