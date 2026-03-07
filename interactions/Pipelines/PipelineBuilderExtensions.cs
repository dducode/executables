using Interactions.Core;
using Interactions.Core.Internal;

namespace Interactions.Pipelines;

public static partial class PipelineBuilderExtensions {

  /// <summary>
  /// Appends middleware step expressed as function with next func.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed executable.</typeparam>
  /// <typeparam name="T2">Input type accepted by current step.</typeparam>
  /// <typeparam name="T3">Input type of downstream function.</typeparam>
  /// <typeparam name="T4">Output type of downstream function.</typeparam>
  /// <typeparam name="T5">Output type produced by current step.</typeparam>
  /// <typeparam name="T6">Output type of the final composed executable.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="middleware">Middleware delegate with downstream function access.</param>
  /// <returns>Builder with updated downstream type pair <typeparamref name="T3" />/<typeparamref name="T4" />.</returns>
  public static PipelineBuilder<T1, T3, T4, T6> Use<T1, T2, T3, T4, T5, T6>(
    this PipelineBuilder<T1, T2, T5, T6> builder,
    Func<T2, Func<T3, T4>, T5> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AnonymousMiddleware<T2, T3, T4, T5>((input, executable) => middleware(input, executable.Execute)));
  }

  /// <summary>
  /// Appends middleware step expressed as function with downstream parameterless function.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed executable.</typeparam>
  /// <typeparam name="T2">Input type accepted by current step.</typeparam>
  /// <typeparam name="T3">Output type of downstream parameterless function.</typeparam>
  /// <typeparam name="T4">Output type produced by current step.</typeparam>
  /// <typeparam name="T5">Output type of the final composed executable.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="middleware">Middleware delegate with downstream parameterless function access.</param>
  /// <returns>Builder with downstream input fixed to <see cref="Unit" />.</returns>
  public static PipelineBuilder<T1, Unit, T3, T5> Use<T1, T2, T3, T4, T5>(
    this PipelineBuilder<T1, T2, T4, T5> builder,
    Func<T2, Func<T3>, T4> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AnonymousMiddleware<T2, Unit, T3, T4>((input, executable) => {
      return middleware(input, () => executable.Execute(default));
    }));
  }

  /// <summary>
  /// Appends middleware step expressed as function with next action.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed executable.</typeparam>
  /// <typeparam name="T2">Input type accepted by current step.</typeparam>
  /// <typeparam name="T3">Input type of downstream action.</typeparam>
  /// <typeparam name="T4">Output type produced by current step.</typeparam>
  /// <typeparam name="T5">Output type of the final composed executable.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="middleware">Middleware delegate with downstream action access.</param>
  /// <returns>Builder with downstream output fixed to <see cref="Unit" />.</returns>
  public static PipelineBuilder<T1, T3, Unit, T5> Use<T1, T2, T3, T4, T5>(
    this PipelineBuilder<T1, T2, T4, T5> builder,
    Func<T2, Action<T3>, T4> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AnonymousMiddleware<T2, T3, Unit, T4>((input, executable) => {
      return middleware(input, i => executable.Execute(i));
    }));
  }

  /// <summary>
  /// Appends middleware step expressed as function with no args.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed executable.</typeparam>
  /// <typeparam name="T2">Input type accepted by current step.</typeparam>
  /// <typeparam name="T3">Output type produced by current step.</typeparam>
  /// <typeparam name="T4">Output type of the final composed executable.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="middleware">Middleware delegate with parameterless downstream action access.</param>
  /// <returns>Builder with downstream input/output fixed to <see cref="Unit" />.</returns>
  public static PipelineBuilder<T1, Unit, Unit, T4> Use<T1, T2, T3, T4>(
    this PipelineBuilder<T1, T2, T3, T4> builder,
    Func<T2, Action, T3> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AnonymousMiddleware<T2, Unit, Unit, T3>((input, executable) => {
      return middleware(input, () => executable.Execute(default));
    }));
  }

  /// <summary>
  /// Appends middleware step to a parameterless chain as function with downstream function.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed executable.</typeparam>
  /// <typeparam name="T2">Output type produced by current step.</typeparam>
  /// <typeparam name="T3">Input type of downstream function.</typeparam>
  /// <typeparam name="T4">Output type of downstream function.</typeparam>
  /// <typeparam name="T5">Output type of the final composed executable.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="middleware">Middleware delegate with downstream function access.</param>
  /// <returns>Builder with updated downstream type pair <typeparamref name="T3"/>/<typeparamref name="T4"/>.</returns>
  public static PipelineBuilder<T1, T3, T4, T5> Use<T1, T2, T3, T4, T5>(
    this PipelineBuilder<T1, Unit, T2, T5> builder,
    Func<Func<T3, T4>, T2> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AnonymousMiddleware<Unit, T3, T4, T2>((_, executable) => middleware(executable.Execute)));
  }

  /// <summary>
  /// Appends middleware step to a parameterless chain as function with downstream parameterless function.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed executable.</typeparam>
  /// <typeparam name="T2">Output type produced by current step.</typeparam>
  /// <typeparam name="T3">Output type of downstream parameterless function.</typeparam>
  /// <typeparam name="T4">Output type of the final composed executable.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="middleware">Middleware delegate with downstream parameterless function access.</param>
  /// <returns>Builder with downstream input fixed to <see cref="Unit" />.</returns>
  public static PipelineBuilder<T1, Unit, T3, T4> Use<T1, T2, T3, T4>(
    this PipelineBuilder<T1, Unit, T2, T4> builder,
    Func<Func<T3>, T2> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AnonymousMiddleware<Unit, Unit, T3, T2>((_, executable) => middleware(() => executable.Execute(default))));
  }

  /// <summary>
  /// Appends middleware step to a parameterless chain as function with downstream action.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed executable.</typeparam>
  /// <typeparam name="T2">Output type produced by current step.</typeparam>
  /// <typeparam name="T3">Input type of downstream action.</typeparam>
  /// <typeparam name="T4">Output type of the final composed executable.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="middleware">Middleware delegate with downstream action access.</param>
  /// <returns>Builder with downstream output fixed to <see cref="Unit" />.</returns>
  public static PipelineBuilder<T1, T3, Unit, T4> Use<T1, T2, T3, T4>(
    this PipelineBuilder<T1, Unit, T2, T4> builder,
    Func<Action<T3>, T2> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AnonymousMiddleware<Unit, T3, Unit, T2>((_, executable) => middleware(i => executable.Execute(i))));
  }

  /// <summary>
  /// Appends middleware step to a parameterless chain as function with downstream parameterless action.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed executable.</typeparam>
  /// <typeparam name="T2">Output type produced by current step.</typeparam>
  /// <typeparam name="T3">Output type of the final composed executable.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="middleware">Middleware delegate with downstream parameterless action access.</param>
  /// <returns>Builder with downstream input/output fixed to <see cref="Unit" />.</returns>
  public static PipelineBuilder<T1, Unit, Unit, T3> Use<T1, T2, T3>(
    this PipelineBuilder<T1, Unit, T2, T3> builder,
    Func<Action, T2> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AnonymousMiddleware<Unit, Unit, Unit, T2>((_, executable) => middleware(() => executable.Execute(default))));
  }

  /// <summary>
  /// Appends middleware step expressed as action with next func.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed executable.</typeparam>
  /// <typeparam name="T2">Input type accepted by current step.</typeparam>
  /// <typeparam name="T3">Input type of downstream function.</typeparam>
  /// <typeparam name="T4">Output type of downstream function.</typeparam>
  /// <typeparam name="T5">Output type of the final composed executable.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="middleware">Middleware action with downstream function access.</param>
  /// <returns>Builder with updated downstream type pair <typeparamref name="T3" />/<typeparamref name="T4" />.</returns>
  public static PipelineBuilder<T1, T3, T4, T5> Use<T1, T2, T3, T4, T5>(
    this PipelineBuilder<T1, T2, Unit, T5> builder,
    Action<T2, Func<T3, T4>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AnonymousMiddleware<T2, T3, T4, Unit>((input, executable) => {
      middleware(input, executable.Execute);
      return default;
    }));
  }

  /// <summary>
  /// Appends middleware step expressed as action with downstream parameterless function.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed executable.</typeparam>
  /// <typeparam name="T2">Input type accepted by current step.</typeparam>
  /// <typeparam name="T3">Output type of downstream parameterless function.</typeparam>
  /// <typeparam name="T4">Output type of the final composed executable.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="middleware">Middleware action with downstream parameterless function access.</param>
  /// <returns>Builder with downstream input fixed to <see cref="Unit" />.</returns>
  public static PipelineBuilder<T1, Unit, T3, T4> Use<T1, T2, T3, T4>(
    this PipelineBuilder<T1, T2, Unit, T4> builder,
    Action<T2, Func<T3>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AnonymousMiddleware<T2, Unit, T3, Unit>((input, executable) => {
      middleware(input, () => executable.Execute(default));
      return default;
    }));
  }

  /// <summary>
  /// Appends middleware step expressed as action with next action.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed executable.</typeparam>
  /// <typeparam name="T2">Input type accepted by current step.</typeparam>
  /// <typeparam name="T3">Input type of downstream action.</typeparam>
  /// <typeparam name="T4">Output type of the final composed executable.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="middleware">Middleware action with downstream typed action access.</param>
  /// <returns>Builder with downstream output fixed to <see cref="Unit" />.</returns>
  public static PipelineBuilder<T1, T3, Unit, T4> Use<T1, T2, T3, T4>(
    this PipelineBuilder<T1, T2, Unit, T4> builder,
    Action<T2, Action<T3>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AnonymousMiddleware<T2, T3, Unit, Unit>((input, executable) => {
      middleware(input, i => executable.Execute(i));
      return default;
    }));
  }

  /// <summary>
  /// Appends middleware step expressed as action with no args.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed executable.</typeparam>
  /// <typeparam name="T2">Input type accepted by current step.</typeparam>
  /// <typeparam name="T3">Output type of the final composed executable.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="middleware">Middleware action with parameterless downstream action access.</param>
  /// <returns>Builder with downstream input/output fixed to <see cref="Unit" />.</returns>
  public static PipelineBuilder<T1, Unit, Unit, T3> Use<T1, T2, T3>(
    this PipelineBuilder<T1, T2, Unit, T3> builder,
    Action<T2, Action> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AnonymousMiddleware<T2, Unit, Unit, Unit>((input, executable) => {
      middleware(input, () => executable.Execute(default));
      return default;
    }));
  }

  /// <summary>
  /// Appends middleware step to a parameterless chain as action with downstream function.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed executable.</typeparam>
  /// <typeparam name="T2">Input type of downstream function.</typeparam>
  /// <typeparam name="T3">Output type of downstream function.</typeparam>
  /// <typeparam name="T4">Output type of the final composed executable.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="middleware">Middleware action with downstream function access.</param>
  /// <returns>Builder with updated downstream type pair <typeparamref name="T2"/>/<typeparamref name="T3"/>.</returns>
  public static PipelineBuilder<T1, T2, T3, T4> Use<T1, T2, T3, T4>(
    this PipelineBuilder<T1, Unit, Unit, T4> builder,
    Action<Func<T2, T3>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AnonymousMiddleware<Unit, T2, T3, Unit>((_, executable) => {
      middleware(executable.Execute);
      return default;
    }));
  }

  /// <summary>
  /// Appends middleware step to a parameterless chain as action with downstream parameterless function.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed executable.</typeparam>
  /// <typeparam name="T2">Output type of downstream parameterless function.</typeparam>
  /// <typeparam name="T3">Output type of the final composed executable.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="middleware">Middleware action with downstream parameterless function access.</param>
  /// <returns>Builder with downstream input fixed to <see cref="Unit" />.</returns>
  public static PipelineBuilder<T1, Unit, T2, T3> Use<T1, T2, T3>(
    this PipelineBuilder<T1, Unit, Unit, T3> builder,
    Action<Func<T2>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AnonymousMiddleware<Unit, Unit, T2, Unit>((_, executable) => {
      middleware(() => executable.Execute(default));
      return default;
    }));
  }

  /// <summary>
  /// Appends middleware step to a parameterless chain as action with downstream typed action.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed executable.</typeparam>
  /// <typeparam name="T2">Input type of downstream action.</typeparam>
  /// <typeparam name="T3">Output type of the final composed executable.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="middleware">Middleware action with downstream typed action access.</param>
  /// <returns>Builder with downstream output fixed to <see cref="Unit" />.</returns>
  public static PipelineBuilder<T1, T2, Unit, T3> Use<T1, T2, T3>(
    this PipelineBuilder<T1, Unit, Unit, T3> builder,
    Action<Action<T2>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AnonymousMiddleware<Unit, T2, Unit, Unit>((_, executable) => {
      middleware(i => executable.Execute(i));
      return default;
    }));
  }

  /// <summary>
  /// Appends middleware step to a parameterless continuation chain.
  /// </summary>
  /// <typeparam name="T1">Input type of the final composed executable.</typeparam>
  /// <typeparam name="T2">Output type of the final composed executable.</typeparam>
  /// <param name="builder">Builder being extended.</param>
  /// <param name="middleware">Middleware action with parameterless downstream action access.</param>
  /// <returns>Builder with downstream input/output fixed to <see cref="Unit" />.</returns>
  public static PipelineBuilder<T1, Unit, Unit, T2> Use<T1, T2>(
    this PipelineBuilder<T1, Unit, Unit, T2> builder,
    Action<Action> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AnonymousMiddleware<Unit, Unit, Unit, Unit>((_, executable) => {
      middleware(() => executable.Execute(default));
      return default;
    }));
  }

  public static IExecutable<T1, T4> End<T1, T2, T3, T4>(this PipelineBuilder<T1, T2, T3, T4> builder, Func<T2, T3> func) {
    return builder.End(Executable.Create(func));
  }

  public static IExecutable<T1, T3> End<T1, T2, T3>(this PipelineBuilder<T1, Unit, T2, T3> builder, Func<T2> func) {
    return builder.End(Executable.Create(func));
  }

  public static IExecutable<T1, T3> End<T1, T2, T3>(this PipelineBuilder<T1, T2, Unit, T3> builder, Action<T2> func) {
    return builder.End(Executable.Create(func));
  }

  public static IExecutable<T1, T3> End<T1, T3>(this PipelineBuilder<T1, Unit, Unit, T3> builder, Action func) {
    return builder.End(Executable.Create(func));
  }

}