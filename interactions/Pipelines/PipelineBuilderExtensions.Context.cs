using Interactions.Context;
using Interactions.Core;
using Interactions.Core.Internal;
using Interactions.Executables;

namespace Interactions.Pipelines;

public static partial class PipelineBuilderExtensions {

  public static PipelineBuilder<T1, T3, T4, T6> Use<T1, T2, T3, T4, T5, T6>(
    this PipelineBuilder<T1, T2, T5, T6> builder,
    Func<T2, ContextFunc<T3, T4>, T5> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AnonymousMiddleware<T2, T3, T4, T5>((input, executor) => middleware(input, executor.Execute)));
  }

  public static PipelineBuilder<T1, Unit, T3, T5> Use<T1, T2, T3, T4, T5>(
    this PipelineBuilder<T1, T2, T4, T5> builder,
    Func<T2, ContextFunc<T3>, T4> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AnonymousMiddleware<T2, Unit, T3, T4>((input, executor) => {
      return middleware(input, init => executor.Execute(default, init));
    }));
  }

  public static PipelineBuilder<T1, T3, Unit, T5> Use<T1, T2, T3, T4, T5>(
    this PipelineBuilder<T1, T2, T4, T5> builder,
    Func<T2, ContextAction<T3>, T4> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AnonymousMiddleware<T2, T3, Unit, T4>((input, executor) => {
      return middleware(input, (i, init) => executor.Execute(i, init));
    }));
  }

  public static PipelineBuilder<T1, Unit, Unit, T4> Use<T1, T2, T3, T4>(
    this PipelineBuilder<T1, T2, T3, T4> builder,
    Func<T2, ContextAction, T3> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AnonymousMiddleware<T2, Unit, Unit, T3>((input, executor) => {
      return middleware(input, init => executor.Execute(default, init));
    }));
  }

  public static PipelineBuilder<T1, T3, T4, T5> Use<T1, T2, T3, T4, T5>(
    this PipelineBuilder<T1, Unit, T2, T5> builder,
    Func<ContextFunc<T3, T4>, T2> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AnonymousMiddleware<Unit, T3, T4, T2>((_, executor) => middleware(executor.Execute)));
  }

  public static PipelineBuilder<T1, Unit, T3, T4> Use<T1, T2, T3, T4>(
    this PipelineBuilder<T1, Unit, T2, T4> builder,
    Func<ContextFunc<T3>, T2> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AnonymousMiddleware<Unit, Unit, T3, T2>((_, executor) => middleware(init => executor.Execute(default, init))));
  }

  public static PipelineBuilder<T1, T3, Unit, T4> Use<T1, T2, T3, T4>(
    this PipelineBuilder<T1, Unit, T2, T4> builder,
    Func<ContextAction<T3>, T2> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AnonymousMiddleware<Unit, T3, Unit, T2>((_, executor) => middleware((i, init) => executor.Execute(i, init))));
  }

  public static PipelineBuilder<T1, Unit, Unit, T3> Use<T1, T2, T3>(
    this PipelineBuilder<T1, Unit, T2, T3> builder,
    Func<ContextAction, T2> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AnonymousMiddleware<Unit, Unit, Unit, T2>((_, executor) => middleware(init => executor.Execute(default, init))));
  }

  public static PipelineBuilder<T1, T3, T4, T5> Use<T1, T2, T3, T4, T5>(
    this PipelineBuilder<T1, T2, Unit, T5> builder,
    Action<T2, ContextFunc<T3, T4>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AnonymousMiddleware<T2, T3, T4, Unit>((input, executor) => {
      middleware(input, executor.Execute);
      return default;
    }));
  }

  public static PipelineBuilder<T1, Unit, T3, T4> Use<T1, T2, T3, T4>(
    this PipelineBuilder<T1, T2, Unit, T4> builder,
    Action<T2, ContextFunc<T3>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AnonymousMiddleware<T2, Unit, T3, Unit>((input, executor) => {
      middleware(input, init => executor.Execute(default, init));
      return default;
    }));
  }

  public static PipelineBuilder<T1, T3, Unit, T4> Use<T1, T2, T3, T4>(
    this PipelineBuilder<T1, T2, Unit, T4> builder,
    Action<T2, ContextAction<T3>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AnonymousMiddleware<T2, T3, Unit, Unit>((input, executor) => {
      middleware(input, (i, init) => executor.Execute(i, init));
      return default;
    }));
  }

  public static PipelineBuilder<T1, Unit, Unit, T3> Use<T1, T2, T3>(
    this PipelineBuilder<T1, T2, Unit, T3> builder,
    Action<T2, ContextAction> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AnonymousMiddleware<T2, Unit, Unit, Unit>((input, executor) => {
      middleware(input, init => executor.Execute(default, init));
      return default;
    }));
  }

  public static PipelineBuilder<T1, T2, T3, T4> Use<T1, T2, T3, T4>(
    this PipelineBuilder<T1, Unit, Unit, T4> builder,
    Action<ContextFunc<T2, T3>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AnonymousMiddleware<Unit, T2, T3, Unit>((_, executor) => {
      middleware(executor.Execute);
      return default;
    }));
  }

  public static PipelineBuilder<T1, Unit, T2, T3> Use<T1, T2, T3>(
    this PipelineBuilder<T1, Unit, Unit, T3> builder,
    Action<ContextFunc<T2>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AnonymousMiddleware<Unit, Unit, T2, Unit>((_, executor) => {
      middleware(init => executor.Execute(default, init));
      return default;
    }));
  }

  public static PipelineBuilder<T1, T2, Unit, T3> Use<T1, T2, T3>(
    this PipelineBuilder<T1, Unit, Unit, T3> builder,
    Action<ContextAction<T2>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AnonymousMiddleware<Unit, T2, Unit, Unit>((_, executor) => {
      middleware((i, init) => executor.Execute(i, init));
      return default;
    }));
  }

  public static PipelineBuilder<T1, Unit, Unit, T2> Use<T1, T2>(
    this PipelineBuilder<T1, Unit, Unit, T2> builder,
    Action<ContextAction> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AnonymousMiddleware<Unit, Unit, Unit, Unit>((_, executor) => {
      middleware(init => executor.Execute(default, init));
      return default;
    }));
  }

}