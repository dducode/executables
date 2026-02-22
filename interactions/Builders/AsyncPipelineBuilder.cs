using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Pipelines;

namespace Interactions.Builders;

/// <summary>
/// Fluent builder that composes asynchronous pipeline middleware steps.
/// </summary>
/// <typeparam name="T1">Input type of the final composed async handler.</typeparam>
/// <typeparam name="T2">Input type expected by current downstream async segment.</typeparam>
/// <typeparam name="T3">Output type produced by current downstream async segment.</typeparam>
/// <typeparam name="T4">Output type of the final composed async handler.</typeparam>
public class AsyncPipelineBuilder<T1, T2, T3, T4> {

  private readonly AsyncPipeline<T1, T2, T3, T4> _pipeline;

  internal AsyncPipelineBuilder(AsyncPipeline<T1, T2, T3, T4> pipeline) {
    _pipeline = pipeline;
  }

  /// <summary>
  /// Appends next async pipeline step and preserves type chain.
  /// </summary>
  /// <typeparam name="T5">Input type expected by the appended downstream async segment.</typeparam>
  /// <typeparam name="T6">Output type produced by the appended downstream async segment.</typeparam>
  /// <param name="pipeline">Async pipeline step appended after current step.</param>
  /// <returns>New builder with updated downstream input and output types.</returns>
  public AsyncPipelineBuilder<T1, T5, T6, T4> Use<T5, T6>(AsyncPipeline<T2, T5, T6, T3> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return new AsyncRecursivePipelineBuilder<T1, T2, T3, T4, T5, T6>(this, new AsyncPipelineBuilder<T2, T5, T6, T3>(pipeline));
  }

  /// <summary>
  /// Finalizes async composition by binding terminal async handler.
  /// </summary>
  /// <param name="handler">Terminal async handler invoked when chain reaches the end.</param>
  /// <returns>Composed async handler that represents the entire pipeline chain.</returns>
  [Pure]
  public virtual AsyncHandler<T1, T4> End(AsyncHandler<T2, T3> handler) {
    ExceptionsHelper.ThrowIfNull(handler, nameof(handler));
    return new AsyncPipelineHandler<T1, T2, T3, T4>(_pipeline, handler);
  }

}
