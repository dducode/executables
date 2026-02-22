using System.Diagnostics.Contracts;
using Interactions.Core;
using Interactions.Pipelines;

namespace Interactions.Builders;

/// <summary>
/// Fluent builder that composes synchronous pipeline middleware steps.
/// </summary>
/// <typeparam name="T1">Input type of the final composed handler.</typeparam>
/// <typeparam name="T2">Input type expected by current downstream segment.</typeparam>
/// <typeparam name="T3">Output type produced by current downstream segment.</typeparam>
/// <typeparam name="T4">Output type of the final composed handler.</typeparam>
public class PipelineBuilder<T1, T2, T3, T4> {

  private readonly Pipeline<T1, T2, T3, T4> _pipeline;

  internal PipelineBuilder(Pipeline<T1, T2, T3, T4> pipeline) {
    _pipeline = pipeline;
  }

  /// <summary>
  /// Appends next pipeline step and keeps fluent composition typed.
  /// </summary>
  /// <typeparam name="T5">Input type expected by the appended downstream segment.</typeparam>
  /// <typeparam name="T6">Output type produced by the appended downstream segment.</typeparam>
  /// <param name="pipeline">Pipeline step appended after current step.</param>
  /// <returns>New builder with updated downstream input and output types.</returns>
  public PipelineBuilder<T1, T5, T6, T4> Use<T5, T6>(Pipeline<T2, T5, T6, T3> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return new RecursivePipelineBuilder<T1, T2, T5, T6, T3, T4>(this, new PipelineBuilder<T2, T5, T6, T3>(pipeline));
  }

  /// <summary>
  /// Finalizes composition by binding terminal handler to pipeline.
  /// </summary>
  /// <param name="handler">Terminal handler invoked when pipeline reaches the end.</param>
  /// <returns>Composed handler that represents the entire pipeline chain.</returns>
  [Pure]
  public virtual Handler<T1, T4> End(Handler<T2, T3> handler) {
    ExceptionsHelper.ThrowIfNull(handler, nameof(handler));
    return new PipelineHandler<T1, T2, T3, T4>(_pipeline, handler);
  }

}
