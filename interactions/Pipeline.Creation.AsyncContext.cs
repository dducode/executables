using Interactions.Context;
using Interactions.Core;
using Interactions.Core.Internal;
using Interactions.Executables;
using Interactions.Pipelines;

namespace Interactions;

public static partial class AsyncPipeline<T1, T4> {

  public static AsyncPipelineBuilder<T1, T2, T3, T4> Use<T2, T3>(AsyncFunc<T1, AsyncContextFunc<T2, T3>, T4> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new AsyncPipelineBuilder<T1, T2, T3, T4>(new AsyncAnonymousMiddleware<T1, T2, T3, T4>((input, executable, token) =>
      middleware(input, executable.Execute, token)
    ));
  }

  public static AsyncPipelineBuilder<T1, Unit, T2, T4> Use<T2>(AsyncFunc<T1, AsyncContextFunc<T2>, T4> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new AsyncPipelineBuilder<T1, Unit, T2, T4>(new AsyncAnonymousMiddleware<T1, Unit, T2, T4>((input, executable, token) => {
      return middleware(input, (init, t) => executable.Execute(default, init, t), token);
    }));
  }

  public static AsyncPipelineBuilder<T1, T2, Unit, T4> Use<T2>(AsyncFunc<T1, AsyncContextAction<T2>, T4> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new AsyncPipelineBuilder<T1, T2, Unit, T4>(new AsyncAnonymousMiddleware<T1, T2, Unit, T4>((input, executable, token) => {
      return middleware(input, async (i, init, t) => await executable.Execute(i, init, t), token);
    }));
  }

  public static AsyncPipelineBuilder<T1, Unit, Unit, T4> Use(AsyncFunc<T1, AsyncContextAction, T4> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new AsyncPipelineBuilder<T1, Unit, Unit, T4>(new AsyncAnonymousMiddleware<T1, Unit, Unit, T4>((input, executable, token) => {
      return middleware(input, async (init, t) => await executable.Execute(default, init, t), token);
    }));
  }

}

public static partial class AsyncPipeline<T> {

  public static AsyncPipelineBuilder<Unit, T1, T2, T> Use<T1, T2>(AsyncFunc<AsyncContextFunc<T1, T2>, T> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new AsyncPipelineBuilder<Unit, T1, T2, T>(new AsyncAnonymousMiddleware<Unit, T1, T2, T>((_, executable, token) => middleware(executable.Execute, token)));
  }

  public static AsyncPipelineBuilder<Unit, Unit, T1, T> Use<T1>(AsyncFunc<AsyncContextFunc<T1>, T> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new AsyncPipelineBuilder<Unit, Unit, T1, T>(new AsyncAnonymousMiddleware<Unit, Unit, T1, T>((_, executable, token) => {
      return middleware((init, t) => executable.Execute(default, init, t), token);
    }));
  }

  public static AsyncPipelineBuilder<Unit, T1, Unit, T> Use<T1>(AsyncFunc<AsyncContextAction<T1>, T> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new AsyncPipelineBuilder<Unit, T1, Unit, T>(new AsyncAnonymousMiddleware<Unit, T1, Unit, T>((_, executable, token) => {
      return middleware(async (i, init, t) => await executable.Execute(i, init, t), token);
    }));
  }

  public static AsyncPipelineBuilder<Unit, Unit, Unit, T> Use(AsyncFunc<AsyncContextAction, T> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new AsyncPipelineBuilder<Unit, Unit, Unit, T>(new AsyncAnonymousMiddleware<Unit, Unit, Unit, T>((_, executable, token) => {
      return middleware(async (init, t) => await executable.Execute(default, init, t), token);
    }));
  }

  public static AsyncPipelineBuilder<T, T1, T2, Unit> Use<T1, T2>(AsyncAction<T, AsyncContextFunc<T1, T2>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new AsyncPipelineBuilder<T, T1, T2, Unit>(new AsyncAnonymousMiddleware<T, T1, T2, Unit>(async (input, executable, token) => {
      await middleware(input, executable.Execute, token);
      return default;
    }));
  }

  public static AsyncPipelineBuilder<T, Unit, T1, Unit> Use<T1>(AsyncAction<T, AsyncContextFunc<T1>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new AsyncPipelineBuilder<T, Unit, T1, Unit>(new AsyncAnonymousMiddleware<T, Unit, T1, Unit>(async (input, executable, token) => {
      await middleware(input, (init, t) => executable.Execute(default, init, t), token);
      return default;
    }));
  }

  public static AsyncPipelineBuilder<T, T1, Unit, Unit> Use<T1>(AsyncAction<T, AsyncContextAction<T1>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new AsyncPipelineBuilder<T, T1, Unit, Unit>(new AsyncAnonymousMiddleware<T, T1, Unit, Unit>(async (input, executable, token) => {
      await middleware(input, async (i, init, t) => await executable.Execute(i, init, t), token);
      return default;
    }));
  }

  public static AsyncPipelineBuilder<T, Unit, Unit, Unit> Use(AsyncAction<T, AsyncContextAction> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new AsyncPipelineBuilder<T, Unit, Unit, Unit>(new AsyncAnonymousMiddleware<T, Unit, Unit, Unit>(async (input, executable, token) => {
      await middleware(input, async (init, t) => await executable.Execute(default, init, t), token);
      return default;
    }));
  }

}

public static partial class AsyncPipeline {

  public static AsyncPipelineBuilder<Unit, T1, T2, Unit> Use<T1, T2>(AsyncAction<AsyncContextFunc<T1, T2>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new AsyncPipelineBuilder<Unit, T1, T2, Unit>(new AsyncAnonymousMiddleware<Unit, T1, T2, Unit>(async (_, executable, token) => {
      await middleware(executable.Execute, token);
      return default;
    }));
  }

  public static AsyncPipelineBuilder<Unit, Unit, T, Unit> Use<T>(AsyncAction<AsyncContextFunc<T>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new AsyncPipelineBuilder<Unit, Unit, T, Unit>(new AsyncAnonymousMiddleware<Unit, Unit, T, Unit>(async (_, executable, token) => {
      await middleware((init, t) => executable.Execute(default, init, t), token);
      return default;
    }));
  }

  public static AsyncPipelineBuilder<Unit, T, Unit, Unit> Use<T>(AsyncAction<AsyncContextAction<T>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new AsyncPipelineBuilder<Unit, T, Unit, Unit>(new AsyncAnonymousMiddleware<Unit, T, Unit, Unit>(async (_, executable, token) => {
      await middleware(async (i, init, t) => await executable.Execute(i, init, t), token);
      return default;
    }));
  }

  public static AsyncPipelineBuilder<Unit, Unit, Unit, Unit> Use(AsyncAction<AsyncContextAction> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new AsyncPipelineBuilder<Unit, Unit, Unit, Unit>(new AsyncAnonymousMiddleware<Unit, Unit, Unit, Unit>(async (_, executable, token) => {
      await middleware(async (init, t) => await executable.Execute(default, init, t), token);
      return default;
    }));
  }

}