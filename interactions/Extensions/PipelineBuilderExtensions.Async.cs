using System.Diagnostics.Contracts;
using Interactions.Builders;
using Interactions.Core;
using Interactions.Pipelines;

namespace Interactions.Extensions;

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
  /// <param name="middleware">Async middleware delegate with downstream function access.</param>
  /// <returns>Builder with updated downstream type pair <typeparamref name="T3" />/<typeparamref name="T4" />.</returns>
  public static AsyncPipelineBuilder<T1, T3, T4, T6> Use<T1, T2, T3, T4, T5, T6>(
    this AsyncPipelineBuilder<T1, T2, T5, T6> builder,
    AsyncFunc<T2, AsyncFunc<T3, T4>, T5> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AsyncAnonymousMiddleware<T2, T3, T4, T5>((input, handler, token) => middleware(input, handler.Handle, token)));
  }

  /// <summary>
  /// Appends async middleware step expressed as function with downstream parameterless async function.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed async handler.</typeparam>
  /// <typeparam name="T2">Input type accepted by current step.</typeparam>
  /// <typeparam name="T3">Output type of downstream parameterless async function.</typeparam>
  /// <typeparam name="T4">Output type produced by current step.</typeparam>
  /// <typeparam name="T5">Output type of the final composed async handler.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="middleware">Async middleware delegate with downstream parameterless async function access.</param>
  /// <returns>Builder with downstream input fixed to <see cref="Unit" />.</returns>
  public static AsyncPipelineBuilder<T1, Unit, T3, T5> Use<T1, T2, T3, T4, T5>(
    this AsyncPipelineBuilder<T1, T2, T4, T5> builder,
    AsyncFunc<T2, AsyncFunc<T3>, T4> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AsyncAnonymousMiddleware<T2, Unit, T3, T4>((input, handler, token) => {
      return middleware(input, t => handler.Handle(default, t), token);
    }));
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
  /// <param name="middleware">Async middleware delegate with downstream action access.</param>
  /// <returns>Builder with downstream output fixed to <see cref="Unit" />.</returns>
  public static AsyncPipelineBuilder<T1, T3, Unit, T5> Use<T1, T2, T3, T4, T5>(
    this AsyncPipelineBuilder<T1, T2, T4, T5> builder,
    AsyncFunc<T2, AsyncAction<T3>, T4> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AsyncAnonymousMiddleware<T2, T3, Unit, T4>((input, handler, token) => {
      return middleware(input, async (i, t) => await handler.Handle(i, t), token);
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
  /// <param name="middleware">Async middleware delegate with parameterless downstream action access.</param>
  /// <returns>Builder with downstream input/output fixed to <see cref="Unit" />.</returns>
  public static AsyncPipelineBuilder<T1, Unit, Unit, T4> Use<T1, T2, T3, T4>(
    this AsyncPipelineBuilder<T1, T2, T3, T4> builder,
    AsyncFunc<T2, AsyncAction, T3> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AsyncAnonymousMiddleware<T2, Unit, Unit, T3>((input, handler, token) => {
      return middleware(input, async t => await handler.Handle(default, t), token);
    }));
  }

  /// <summary>
  /// Appends async middleware step to a parameterless chain as function with downstream async function.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed async handler.</typeparam>
  /// <typeparam name="T2">Output type produced by current step.</typeparam>
  /// <typeparam name="T3">Input type of downstream async function.</typeparam>
  /// <typeparam name="T4">Output type of downstream async function.</typeparam>
  /// <typeparam name="T5">Output type of the final composed async handler.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="middleware">Async middleware delegate with downstream async function access.</param>
  /// <returns>Builder with updated downstream type pair <typeparamref name="T3"/>/<typeparamref name="T4"/>.</returns>
  public static AsyncPipelineBuilder<T1, T3, T4, T5> Use<T1, T2, T3, T4, T5>(
    this AsyncPipelineBuilder<T1, Unit, T2, T5> builder,
    AsyncFunc<AsyncFunc<T3, T4>, T2> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AsyncAnonymousMiddleware<Unit, T3, T4, T2>((_, handler, token) => middleware(handler.Handle, token)));
  }

  /// <summary>
  /// Appends async middleware step to a parameterless chain as function with downstream parameterless async function.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed async handler.</typeparam>
  /// <typeparam name="T2">Output type produced by current step.</typeparam>
  /// <typeparam name="T3">Output type of downstream parameterless async function.</typeparam>
  /// <typeparam name="T4">Output type of the final composed async handler.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="middleware">Async middleware delegate with downstream parameterless async function access.</param>
  /// <returns>Builder with downstream input fixed to <see cref="Unit" />.</returns>
  public static AsyncPipelineBuilder<T1, Unit, T3, T4> Use<T1, T2, T3, T4>(
    this AsyncPipelineBuilder<T1, Unit, T2, T4> builder,
    AsyncFunc<AsyncFunc<T3>, T2> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AsyncAnonymousMiddleware<Unit, Unit, T3, T2>((_, handler, token) => {
      return middleware(t => handler.Handle(default, t), token);
    }));
  }

  /// <summary>
  /// Appends async middleware step to a parameterless chain as function with downstream async action.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed async handler.</typeparam>
  /// <typeparam name="T2">Output type produced by current step.</typeparam>
  /// <typeparam name="T3">Input type of downstream async action.</typeparam>
  /// <typeparam name="T4">Output type of the final composed async handler.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="middleware">Async middleware delegate with downstream async action access.</param>
  /// <returns>Builder with downstream output fixed to <see cref="Unit" />.</returns>
  public static AsyncPipelineBuilder<T1, T3, Unit, T4> Use<T1, T2, T3, T4>(
    this AsyncPipelineBuilder<T1, Unit, T2, T4> builder,
    AsyncFunc<AsyncAction<T3>, T2> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AsyncAnonymousMiddleware<Unit, T3, Unit, T2>((_, handler, token) => {
      return middleware(async (i, t) => await handler.Handle(i, t), token);
    }));
  }

  /// <summary>
  /// Appends async middleware step to a parameterless chain as function with downstream parameterless async action.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed async handler.</typeparam>
  /// <typeparam name="T2">Output type produced by current step.</typeparam>
  /// <typeparam name="T3">Output type of the final composed async handler.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="middleware">Async middleware delegate with downstream parameterless async action access.</param>
  /// <returns>Builder with downstream input/output fixed to <see cref="Unit" />.</returns>
  public static AsyncPipelineBuilder<T1, Unit, Unit, T3> Use<T1, T2, T3>(
    this AsyncPipelineBuilder<T1, Unit, T2, T3> builder,
    AsyncFunc<AsyncAction, T2> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AsyncAnonymousMiddleware<Unit, Unit, Unit, T2>((_, handler, token) => {
      return middleware(async t => await handler.Handle(default, t), token);
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
  /// <param name="middleware">Async middleware action with downstream function access.</param>
  /// <returns>Builder with updated downstream type pair <typeparamref name="T3" />/<typeparamref name="T4" />.</returns>
  public static AsyncPipelineBuilder<T1, T3, T4, T5> Use<T1, T2, T3, T4, T5>(
    this AsyncPipelineBuilder<T1, T2, Unit, T5> builder,
    AsyncAction<T2, AsyncFunc<T3, T4>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AsyncAnonymousMiddleware<T2, T3, T4, Unit>(async (input, handler, token) => {
      await middleware(input, handler.Handle, token);
      return default;
    }));
  }

  /// <summary>
  /// Appends async middleware step expressed as action with downstream parameterless async function.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed async handler.</typeparam>
  /// <typeparam name="T2">Input type accepted by current step.</typeparam>
  /// <typeparam name="T3">Output type of downstream parameterless async function.</typeparam>
  /// <typeparam name="T4">Output type of the final composed async handler.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="middleware">Async middleware action with downstream parameterless async function access.</param>
  /// <returns>Builder with downstream input fixed to <see cref="Unit" />.</returns>
  public static AsyncPipelineBuilder<T1, Unit, T3, T4> Use<T1, T2, T3, T4>(
    this AsyncPipelineBuilder<T1, T2, Unit, T4> builder,
    AsyncAction<T2, AsyncFunc<T3>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AsyncAnonymousMiddleware<T2, Unit, T3, Unit>(async (input, handler, token) => {
      await middleware(input, t => handler.Handle(default, t), token);
      return default;
    }));
  }

  /// <summary>
  /// Appends async middleware step expressed as action with action.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed async handler.</typeparam>
  /// <typeparam name="T2">Input type accepted by current step.</typeparam>
  /// <typeparam name="T3">Input type of downstream async action.</typeparam>
  /// <typeparam name="T4">Output type of the final composed async handler.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="middleware">Async middleware action with downstream typed action access.</param>
  /// <returns>Builder with downstream output fixed to <see cref="Unit" />.</returns>
  public static AsyncPipelineBuilder<T1, T3, Unit, T4> Use<T1, T2, T3, T4>(
    this AsyncPipelineBuilder<T1, T2, Unit, T4> builder,
    AsyncAction<T2, AsyncAction<T3>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AsyncAnonymousMiddleware<T2, T3, Unit, Unit>(async (input, handler, token) => {
      await middleware(input, async (i, t) => await handler.Handle(i, t), token);
      return default;
    }));
  }

  /// <summary>
  /// Appends async middleware step expressed as action with no args.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed async handler.</typeparam>
  /// <typeparam name="T2">Input type accepted by current step.</typeparam>
  /// <typeparam name="T3">Output type of the final composed async handler.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="middleware">Async middleware action with parameterless downstream action access.</param>
  /// <returns>Builder with downstream input/output fixed to <see cref="Unit" />.</returns>
  public static AsyncPipelineBuilder<T1, Unit, Unit, T3> Use<T1, T2, T3>(
    this AsyncPipelineBuilder<T1, T2, Unit, T3> builder,
    AsyncAction<T2, AsyncAction> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AsyncAnonymousMiddleware<T2, Unit, Unit, Unit>(async (input, handler, token) => {
      await middleware(input, async t => await handler.Handle(default, t), token);
      return default;
    }));
  }

  /// <summary>
  /// Appends async middleware step to a parameterless chain as action with downstream async function.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed async handler.</typeparam>
  /// <typeparam name="T2">Input type of downstream async function.</typeparam>
  /// <typeparam name="T3">Output type of downstream async function.</typeparam>
  /// <typeparam name="T4">Output type of the final composed async handler.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="middleware">Async middleware action with downstream async function access.</param>
  /// <returns>Builder with updated downstream type pair <typeparamref name="T2"/>/<typeparamref name="T3"/>.</returns>
  public static AsyncPipelineBuilder<T1, T2, T3, T4> Use<T1, T2, T3, T4>(
    this AsyncPipelineBuilder<T1, Unit, Unit, T4> builder,
    AsyncAction<AsyncFunc<T2, T3>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AsyncAnonymousMiddleware<Unit, T2, T3, Unit>(async (_, handler, token) => {
      await middleware(handler.Handle, token);
      return default;
    }));
  }

  /// <summary>
  /// Appends async middleware step to a parameterless chain as action with downstream parameterless async function.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed async handler.</typeparam>
  /// <typeparam name="T2">Output type of downstream parameterless async function.</typeparam>
  /// <typeparam name="T3">Output type of the final composed async handler.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="middleware">Async middleware action with downstream parameterless async function access.</param>
  /// <returns>Builder with downstream input fixed to <see cref="Unit" />.</returns>
  public static AsyncPipelineBuilder<T1, Unit, T2, T3> Use<T1, T2, T3>(
    this AsyncPipelineBuilder<T1, Unit, Unit, T3> builder,
    AsyncAction<AsyncFunc<T2>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AsyncAnonymousMiddleware<Unit, Unit, T2, Unit>(async (_, handler, token) => {
      await middleware(t => handler.Handle(default, t), token);
      return default;
    }));
  }

  /// <summary>
  /// Appends async middleware step to a parameterless chain as action with downstream async action.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed async handler.</typeparam>
  /// <typeparam name="T2">Input type of downstream async action.</typeparam>
  /// <typeparam name="T3">Output type of the final composed async handler.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="middleware">Async middleware action with downstream async action access.</param>
  /// <returns>Builder with downstream output fixed to <see cref="Unit" />.</returns>
  public static AsyncPipelineBuilder<T1, T2, Unit, T3> Use<T1, T2, T3>(
    this AsyncPipelineBuilder<T1, Unit, Unit, T3> builder,
    AsyncAction<AsyncAction<T2>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AsyncAnonymousMiddleware<Unit, T2, Unit, Unit>(async (_, handler, token) => {
      await middleware(async (i, t) => await handler.Handle(i, t), token);
      return default;
    }));
  }

  /// <summary>
  /// Appends async middleware step to parameterless continuation chain.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed async handler.</typeparam>
  /// <typeparam name="T2">Output type of the final composed async handler.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="middleware">Async middleware action with parameterless downstream action access.</param>
  /// <returns>Builder with downstream input/output fixed to <see cref="Unit" />.</returns>
  public static AsyncPipelineBuilder<T1, Unit, Unit, T2> Use<T1, T2>(
    this AsyncPipelineBuilder<T1, Unit, Unit, T2> builder,
    AsyncAction<AsyncAction> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AsyncAnonymousMiddleware<Unit, Unit, Unit, Unit>(async (_, handler, token) => {
      await middleware(async t => await handler.Handle(default, t), token);
      return default;
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
  /// <param name="func">Terminal async function used as final handler.</param>
  /// <returns>Composed async handler ready to be attached to async handleable.</returns>
  [Pure]
  public static AsyncHandler<T1, T4> End<T1, T2, T3, T4>(this AsyncPipelineBuilder<T1, T2, T3, T4> builder, AsyncFunc<T2, T3> func) {
    return builder.End(Handler.FromAsyncMethod(func));
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
    return builder.End(Handler.FromAsyncMethod(action));
  }

  /// <summary>
  /// Finalizes async pipeline by converting parameterless async function to handler.
  /// </summary>
  /// <typeparam name="T1">Input type of the composed async handler.</typeparam>
  /// <typeparam name="T2">Output type returned by terminal parameterless async function.</typeparam>
  /// <typeparam name="T3">Output type of the composed async handler.</typeparam>
  /// <param name="builder">Builder being finalized.</param>
  /// <param name="func">Terminal parameterless async function used as final handler.</param>
  /// <returns>Composed async handler ready to be attached to async handleable.</returns>
  [Pure]
  public static AsyncHandler<T1, T3> End<T1, T2, T3>(this AsyncPipelineBuilder<T1, Unit, T2, T3> builder, AsyncFunc<T2> func) {
    return builder.End(Handler.FromAsyncMethod(func));
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
    return builder.End(Handler.FromAsyncMethod(action));
  }

}