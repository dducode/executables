using System.Diagnostics.Contracts;
using Executables.Core.Pipelines;
using Executables.Internal;
using Executables.Operations;

namespace Executables.Pipelines;

/// <summary>
/// Fluent builder that composes synchronous pipeline middleware steps.
/// </summary>
/// <typeparam name="T1">Input type of the final composed handler.</typeparam>
/// <typeparam name="T2">Input type expected by current downstream segment.</typeparam>
/// <typeparam name="T3">Output type produced by current downstream segment.</typeparam>
/// <typeparam name="T4">Output type of the final composed handler.</typeparam>
public class PipelineBuilder<T1, T2, T3, T4> {

  private readonly Middleware<T1, T2, T3, T4> _middleware;

  internal PipelineBuilder(Middleware<T1, T2, T3, T4> middleware) {
    _middleware = middleware;
  }

  /// <summary>
  /// Appends next pipeline step and keeps fluent composition typed.
  /// </summary>
  /// <typeparam name="T5">Input type expected by the appended downstream segment.</typeparam>
  /// <typeparam name="T6">Output type produced by the appended downstream segment.</typeparam>
  /// <param name="middleware">Pipeline step appended after current step.</param>
  /// <returns>New builder with updated downstream input and output types.</returns>
  public PipelineBuilder<T1, T5, T6, T4> Use<T5, T6>(Middleware<T2, T5, T6, T3> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new RecursivePipelineBuilder<T1, T2, T5, T6, T3, T4>(this, new PipelineBuilder<T2, T5, T6, T3>(middleware));
  }

  /// <summary>
  /// Finalizes composition by binding a terminal executor to the pipeline.
  /// </summary>
  /// <param name="executor">Terminal executor invoked when the pipeline reaches the end.</param>
  /// <returns>Composed executor that represents the entire pipeline chain.</returns>
  /// <exception cref="ArgumentNullException"><paramref name="executor"/> is <see langword="null"/>.</exception>
  [Pure]
  public virtual IExecutor<T1, T4> End(IExecutor<T2, T3> executor) {
    ExceptionsHelper.ThrowIfNull(executor, nameof(executor));
    return executor.Apply(_middleware);
  }

}