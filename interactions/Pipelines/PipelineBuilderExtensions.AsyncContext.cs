using Interactions.Core;

namespace Interactions.Pipelines;

public static partial class PipelineBuilderExtensions {

  public static AsyncPipelineBuilder<T1, T3, T4, T6> Use<T1, T2, T3, T4, T5, T6>(
    this AsyncPipelineBuilder<T1, T2, T5, T6> builder,
    AsyncFunc<T2, AsyncContextFunc<T3, T4>, T5> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AsyncAnonymousMiddleware<T2, T3, T4, T5>((input, executable, token) => middleware(input, executable.Execute, token)));
  }

  public static AsyncPipelineBuilder<T1, Unit, T3, T5> Use<T1, T2, T3, T4, T5>(
    this AsyncPipelineBuilder<T1, T2, T4, T5> builder,
    AsyncFunc<T2, AsyncContextFunc<T3>, T4> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AsyncAnonymousMiddleware<T2, Unit, T3, T4>((input, executable, token) => {
      return middleware(input, (init, t) => executable.Execute(default, init, t), token);
    }));
  }

  public static AsyncPipelineBuilder<T1, T3, Unit, T5> Use<T1, T2, T3, T4, T5>(
    this AsyncPipelineBuilder<T1, T2, T4, T5> builder,
    AsyncFunc<T2, AsyncContextAction<T3>, T4> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AsyncAnonymousMiddleware<T2, T3, Unit, T4>((input, executable, token) => {
      return middleware(input, async (i, init, t) => await executable.Execute(i, init, t), token);
    }));
  }

  public static AsyncPipelineBuilder<T1, Unit, Unit, T4> Use<T1, T2, T3, T4>(
    this AsyncPipelineBuilder<T1, T2, T3, T4> builder,
    AsyncFunc<T2, AsyncContextAction, T3> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AsyncAnonymousMiddleware<T2, Unit, Unit, T3>((input, executable, token) => {
      return middleware(input, async (init, t) => await executable.Execute(default, init, t), token);
    }));
  }

  public static AsyncPipelineBuilder<T1, T3, T4, T5> Use<T1, T2, T3, T4, T5>(
    this AsyncPipelineBuilder<T1, Unit, T2, T5> builder,
    AsyncFunc<AsyncContextFunc<T3, T4>, T2> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AsyncAnonymousMiddleware<Unit, T3, T4, T2>((_, executable, token) => middleware(executable.Execute, token)));
  }

  public static AsyncPipelineBuilder<T1, Unit, T3, T4> Use<T1, T2, T3, T4>(
    this AsyncPipelineBuilder<T1, Unit, T2, T4> builder,
    AsyncFunc<AsyncContextFunc<T3>, T2> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AsyncAnonymousMiddleware<Unit, Unit, T3, T2>((_, executable, token) => {
      return middleware((init, t) => executable.Execute(default, init, t), token);
    }));
  }

  public static AsyncPipelineBuilder<T1, T3, Unit, T4> Use<T1, T2, T3, T4>(
    this AsyncPipelineBuilder<T1, Unit, T2, T4> builder,
    AsyncFunc<AsyncContextAction<T3>, T2> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AsyncAnonymousMiddleware<Unit, T3, Unit, T2>((_, executable, token) => {
      return middleware(async (i, init, t) => await executable.Execute(i, init, t), token);
    }));
  }

  public static AsyncPipelineBuilder<T1, Unit, Unit, T3> Use<T1, T2, T3>(
    this AsyncPipelineBuilder<T1, Unit, T2, T3> builder,
    AsyncFunc<AsyncContextAction, T2> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AsyncAnonymousMiddleware<Unit, Unit, Unit, T2>((_, executable, token) => {
      return middleware(async (init, t) => await executable.Execute(default, init, t), token);
    }));
  }

  public static AsyncPipelineBuilder<T1, T3, T4, T5> Use<T1, T2, T3, T4, T5>(
    this AsyncPipelineBuilder<T1, T2, Unit, T5> builder,
    AsyncAction<T2, AsyncContextFunc<T3, T4>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AsyncAnonymousMiddleware<T2, T3, T4, Unit>(async (input, executable, token) => {
      await middleware(input, executable.Execute, token);
      return default;
    }));
  }

  public static AsyncPipelineBuilder<T1, Unit, T3, T4> Use<T1, T2, T3, T4>(
    this AsyncPipelineBuilder<T1, T2, Unit, T4> builder,
    AsyncAction<T2, AsyncContextFunc<T3>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AsyncAnonymousMiddleware<T2, Unit, T3, Unit>(async (input, executable, token) => {
      await middleware(input, (init, t) => executable.Execute(default, init, t), token);
      return default;
    }));
  }

  public static AsyncPipelineBuilder<T1, T3, Unit, T4> Use<T1, T2, T3, T4>(
    this AsyncPipelineBuilder<T1, T2, Unit, T4> builder,
    AsyncAction<T2, AsyncContextAction<T3>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AsyncAnonymousMiddleware<T2, T3, Unit, Unit>(async (input, executable, token) => {
      await middleware(input, async (i, init, t) => await executable.Execute(i, init, t), token);
      return default;
    }));
  }

  public static AsyncPipelineBuilder<T1, Unit, Unit, T3> Use<T1, T2, T3>(
    this AsyncPipelineBuilder<T1, T2, Unit, T3> builder,
    AsyncAction<T2, AsyncContextAction> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AsyncAnonymousMiddleware<T2, Unit, Unit, Unit>(async (input, executable, token) => {
      await middleware(input, async (init, t) => await executable.Execute(default, init, t), token);
      return default;
    }));
  }

  public static AsyncPipelineBuilder<T1, T2, T3, T4> Use<T1, T2, T3, T4>(
    this AsyncPipelineBuilder<T1, Unit, Unit, T4> builder,
    AsyncAction<AsyncContextFunc<T2, T3>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AsyncAnonymousMiddleware<Unit, T2, T3, Unit>(async (_, executable, token) => {
      await middleware(executable.Execute, token);
      return default;
    }));
  }

  public static AsyncPipelineBuilder<T1, Unit, T2, T3> Use<T1, T2, T3>(
    this AsyncPipelineBuilder<T1, Unit, Unit, T3> builder,
    AsyncAction<AsyncContextFunc<T2>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AsyncAnonymousMiddleware<Unit, Unit, T2, Unit>(async (_, executable, token) => {
      await middleware((init, t) => executable.Execute(default, init, t), token);
      return default;
    }));
  }

  public static AsyncPipelineBuilder<T1, T2, Unit, T3> Use<T1, T2, T3>(
    this AsyncPipelineBuilder<T1, Unit, Unit, T3> builder,
    AsyncAction<AsyncContextAction<T2>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AsyncAnonymousMiddleware<Unit, T2, Unit, Unit>(async (_, executable, token) => {
      await middleware(async (i, init, t) => await executable.Execute(i, init, t), token);
      return default;
    }));
  }

  public static AsyncPipelineBuilder<T1, Unit, Unit, T2> Use<T1, T2>(
    this AsyncPipelineBuilder<T1, Unit, Unit, T2> builder,
    AsyncAction<AsyncContextAction> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return builder.Use(new AsyncAnonymousMiddleware<Unit, Unit, Unit, Unit>(async (_, executable, token) => {
      await middleware(async (init, t) => await executable.Execute(default, init, t), token);
      return default;
    }));
  }

}