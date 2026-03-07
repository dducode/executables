using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Core.Executables;

namespace Interactions.Pipelines;

/// <summary>
/// Fluent builder that composes asynchronous pipeline middleware steps.
/// </summary>
/// <typeparam name="T1">Input type of the final composed async handler.</typeparam>
/// <typeparam name="T2">Input type expected by current downstream async segment.</typeparam>
/// <typeparam name="T3">Output type produced by current downstream async segment.</typeparam>
/// <typeparam name="T4">Output type of the final composed async handler.</typeparam>
public class AsyncPipelineBuilder<T1, T2, T3, T4> {

  private readonly AsyncMiddleware<T1, T2, T3, T4> _middleware;

  internal AsyncPipelineBuilder(AsyncMiddleware<T1, T2, T3, T4> middleware) {
    _middleware = middleware;
  }

  /// <summary>
  /// Appends next async pipeline step and preserves type chain.
  /// </summary>
  /// <typeparam name="T5">Input type expected by the appended downstream async segment.</typeparam>
  /// <typeparam name="T6">Output type produced by the appended downstream async segment.</typeparam>
  /// <param name="middleware">Async pipeline step appended after current step.</param>
  /// <returns>New builder with updated downstream input and output types.</returns>
  public AsyncPipelineBuilder<T1, T5, T6, T4> Use<T5, T6>(AsyncMiddleware<T2, T5, T6, T3> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new AsyncRecursivePipelineBuilder<T1, T2, T3, T4, T5, T6>(this, new AsyncPipelineBuilder<T2, T5, T6, T3>(middleware));
  }

  /// <summary>
  /// Finalizes async composition by binding terminal async executable
  /// </summary>
  /// <param name="executable">Terminal async executable invoked when chain reaches the end.</param>
  /// <returns>Composed async executable that represents the entire pipeline chain.</returns>
  [Pure]
  public virtual IAsyncExecutable<T1, T4> End(IAsyncExecutable<T2, T3> executable) {
    ExceptionsHelper.ThrowIfNull(executable, nameof(executable));
    return new AsyncMiddlewareExecutable<T1, T2, T3, T4>(_middleware, executable);
  }

}