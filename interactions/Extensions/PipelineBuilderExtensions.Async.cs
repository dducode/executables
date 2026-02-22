using System.Diagnostics.Contracts;
using Interactions.Builders;
using Interactions.Core;
using Interactions.Pipelines;

namespace Interactions.Extensions;

/// <summary>
/// Convenience overloads for composing asynchronous pipeline builders.
/// </summary>
/// <remarks>
/// These overloads allow defining middleware with async delegates instead of
/// constructing <see cref="AsyncPipeline{T1,T2,T3,T4}" /> instances manually.
/// </remarks>
public static partial class PipelineBuilderExtensions {

  /// <summary>
  /// Appends async middleware step expressed as function with next func.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed async handler.</typeparam>
  /// <typeparam name="T2">Input type accepted by current step.</typeparam>
  /// <typeparam name="T3">Input type of downstream async function.</typeparam>
  /// <typeparam name="T4">Output type of downstream async function.</typeparam>
  /// <typeparam name="T5">Output type produced by current step.</typeparam>
  /// <typeparam name="T6">Output type of the final composed async handler.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="pipeline">Async middleware delegate with downstream function access.</param>
  /// <returns>Builder with updated downstream type pair <typeparamref name="T3" />/<typeparamref name="T4" />.</returns>
  public static AsyncPipelineBuilder<T1, T3, T4, T6> Use<T1, T2, T3, T4, T5, T6>(
    this AsyncPipelineBuilder<T1, T2, T5, T6> builder,
    AsyncFunc<T2, AsyncFunc<T3, T4>, T5> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return builder.Use(new AsyncAnonymousPipeline<T2, T3, T4, T5>((input, handler, token) => pipeline(input, handler.Handle, token)));
  }

  /// <summary>
  /// Appends async middleware step expressed as function with action.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed async handler.</typeparam>
  /// <typeparam name="T2">Input type accepted by current step.</typeparam>
  /// <typeparam name="T3">Input type of downstream async action.</typeparam>
  /// <typeparam name="T4">Output type produced by current step.</typeparam>
  /// <typeparam name="T5">Output type of the final composed async handler.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="pipeline">Async middleware delegate with downstream action access.</param>
  /// <returns>Builder with downstream output fixed to <see cref="Unit" />.</returns>
  public static AsyncPipelineBuilder<T1, T3, Unit, T5> Use<T1, T2, T3, T4, T5>(
    this AsyncPipelineBuilder<T1, T2, T4, T5> builder,
    AsyncFunc<T2, AsyncAction<T3>, T4> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return builder.Use(new AsyncAnonymousPipeline<T2, T3, Unit, T4>((input, handler, token) => {
      return pipeline(input, async (i, t) => await handler.Handle(i, t), token);
    }));
  }

  /// <summary>
  /// Appends async middleware step expressed as function with no args.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed async handler.</typeparam>
  /// <typeparam name="T2">Input type accepted by current step.</typeparam>
  /// <typeparam name="T3">Output type produced by current step.</typeparam>
  /// <typeparam name="T4">Output type of the final composed async handler.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="pipeline">Async middleware delegate with parameterless downstream action access.</param>
  /// <returns>Builder with downstream input/output fixed to <see cref="Unit" />.</returns>
  public static AsyncPipelineBuilder<T1, Unit, Unit, T4> Use<T1, T2, T3, T4>(
    this AsyncPipelineBuilder<T1, T2, T3, T4> builder,
    AsyncFunc<T2, AsyncAction, T3> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return builder.Use(new AsyncAnonymousPipeline<T2, Unit, Unit, T3>((input, handler, token) => {
      return pipeline(input, async t => await handler.Handle(default, t), token);
    }));
  }

  /// <summary>
  /// Appends async middleware step expressed as action with next func.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed async handler.</typeparam>
  /// <typeparam name="T2">Input type accepted by current step.</typeparam>
  /// <typeparam name="T3">Input type of downstream async function.</typeparam>
  /// <typeparam name="T4">Output type of downstream async function.</typeparam>
  /// <typeparam name="T5">Output type of the final composed async handler.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="pipeline">Async middleware action with downstream function access.</param>
  /// <returns>Builder with updated downstream type pair <typeparamref name="T3" />/<typeparamref name="T4" />.</returns>
  public static AsyncPipelineBuilder<T1, T3, T4, T5> Use<T1, T2, T3, T4, T5>(
    this AsyncPipelineBuilder<T1, T2, Unit, T5> builder,
    AsyncAction<T2, AsyncFunc<T3, T4>> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return builder.Use(new AsyncAnonymousPipeline<T2, T3, T4>((input, handler, token) => pipeline(input, handler.Handle, token)));
  }

  /// <summary>
  /// Appends async middleware step expressed as action with action.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed async handler.</typeparam>
  /// <typeparam name="T2">Input type accepted by current step.</typeparam>
  /// <typeparam name="T3">Input type of downstream async action.</typeparam>
  /// <typeparam name="T4">Output type of the final composed async handler.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="pipeline">Async middleware action with downstream typed action access.</param>
  /// <returns>Builder with downstream output fixed to <see cref="Unit" />.</returns>
  public static AsyncPipelineBuilder<T1, T3, Unit, T4> Use<T1, T2, T3, T4>(
    this AsyncPipelineBuilder<T1, T2, Unit, T4> builder,
    AsyncAction<T2, AsyncAction<T3>> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return builder.Use(new AsyncAnonymousPipeline<T2, T3, Unit>((input, handler, token) => {
      return pipeline(input, async (i, t) => await handler.Handle(i, t), token);
    }));
  }

  /// <summary>
  /// Appends async middleware step expressed as action with no args.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed async handler.</typeparam>
  /// <typeparam name="T2">Input type accepted by current step.</typeparam>
  /// <typeparam name="T3">Output type of the final composed async handler.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="pipeline">Async middleware action with parameterless downstream action access.</param>
  /// <returns>Builder with downstream input/output fixed to <see cref="Unit" />.</returns>
  public static AsyncPipelineBuilder<T1, Unit, Unit, T3> Use<T1, T2, T3>(
    this AsyncPipelineBuilder<T1, T2, Unit, T3> builder,
    AsyncAction<T2, AsyncAction> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return builder.Use(new AsyncAnonymousPipeline<T2, Unit, Unit>((input, handler, token) => {
      return pipeline(input, async t => await handler.Handle(default, t), token);
    }));
  }

  /// <summary>
  /// Appends async middleware step to parameterless continuation chain.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed async handler.</typeparam>
  /// <typeparam name="T2">Output type of the final composed async handler.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="pipeline">Async middleware action with parameterless downstream action access.</param>
  /// <returns>Builder with downstream input/output fixed to <see cref="Unit" />.</returns>
  public static AsyncPipelineBuilder<T1, Unit, Unit, T2> Use<T1, T2>(
    this AsyncPipelineBuilder<T1, Unit, Unit, T2> builder,
    AsyncAction<AsyncAction> pipeline) {
    ExceptionsHelper.ThrowIfNull(pipeline, nameof(pipeline));
    return builder.Use(new AsyncAnonymousPipeline<Unit, Unit, Unit>((_, handler, token) => {
      return pipeline(async t => await handler.Handle(default, t), token);
    }));
  }

  /// <summary>
  /// Finalizes async pipeline by converting function to handler.
  /// </summary>
  /// <typeparam name="T1">Input type of the composed async handler.</typeparam>
  /// <typeparam name="T2">Input type accepted by terminal async function.</typeparam>
  /// <typeparam name="T3">Output type returned by terminal async function.</typeparam>
  /// <typeparam name="T4">Output type of the composed async handler.</typeparam>
  /// <param name="builder">Builder being finalized.</param>
  /// <param name="action">Terminal async function used as final handler.</param>
  /// <returns>Composed async handler ready to be attached to async handleable.</returns>
  [Pure]
  public static AsyncHandler<T1, T4> End<T1, T2, T3, T4>(this AsyncPipelineBuilder<T1, T2, T3, T4> builder, AsyncFunc<T2, T3> action) {
    return builder.End(Handler.FromMethod(action));
  }

  /// <summary>
  /// Finalizes async pipeline by converting action to handler.
  /// </summary>
  /// <typeparam name="T1">Input type of the composed async handler.</typeparam>
  /// <typeparam name="T2">Input type accepted by terminal async action.</typeparam>
  /// <typeparam name="T3">Output type of the composed async handler.</typeparam>
  /// <param name="builder">Builder being finalized.</param>
  /// <param name="action">Terminal async action used as final handler.</param>
  /// <returns>Composed async handler ready to be attached to async handleable.</returns>
  [Pure]
  public static AsyncHandler<T1, T3> End<T1, T2, T3>(this AsyncPipelineBuilder<T1, T2, Unit, T3> builder, AsyncAction<T2> action) {
    return builder.End(Handler.FromMethod(action));
  }

  /// <summary>
  /// Finalizes async pipeline from a parameterless async action.
  /// </summary>
  /// <typeparam name="T1">Input type of the composed async handler.</typeparam>
  /// <typeparam name="T2">Output type of the composed async handler.</typeparam>
  /// <param name="builder">Builder being finalized.</param>
  /// <param name="action">Terminal parameterless async action used as final handler.</param>
  /// <returns>Composed async handler ready to be attached to async handleable.</returns>
  [Pure]
  public static AsyncHandler<T1, T2> End<T1, T2>(this AsyncPipelineBuilder<T1, Unit, Unit, T2> builder, AsyncAction action) {
    return builder.End(Handler.FromMethod(action));
  }

}