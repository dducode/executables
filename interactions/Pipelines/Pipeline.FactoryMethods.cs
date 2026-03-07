using Interactions.Core;
using Interactions.Core.Internal;

namespace Interactions.Pipelines;

/// <summary>
/// Entry point for building pipelines that return a value.
/// </summary>
/// <typeparam name="T1">Input type of the final composed executable.</typeparam>
/// <typeparam name="T4">Output type of the final composed executable.</typeparam>
/// <remarks>
/// For overloads of <c>Use</c> method, it is recommended
/// to explicitly specify lambda parameter types to avoid overload-resolution ambiguity.
/// <example>
/// <code>
/// var executable = Pipeline&lt;string, string&gt;
///   .Use((string time, Func&lt;TimeSpan, TimeSpan&gt; next) =&gt; {
///     var parsed = TimeSpan.Parse(time);
///     return next(parsed).ToString();
///   })
///   .End(Executable.Create(TimeSpan parsed =&gt; {
///     Console.WriteLine(parsed);
///     return parsed;
///   }));
/// </code>
/// </example>
/// </remarks>
public static partial class Pipeline<T1, T4> {

  /// <summary>
  /// Starts a pipeline from a step that receives downstream call as <see cref="Func{T2,T3}" />.
  /// </summary>
  /// <typeparam name="T2">Input type expected by the downstream function.</typeparam>
  /// <typeparam name="T3">Output type returned by the downstream function.</typeparam>
  /// <param name="middleware">Middleware function that can invoke downstream function.</param>
  /// <returns>Builder for appending next middleware steps and terminal executable.</returns>
  public static PipelineBuilder<T1, T2, T3, T4> Use<T2, T3>(Func<T1, Func<T2, T3>, T4> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new PipelineBuilder<T1, T2, T3, T4>(new AnonymousMiddleware<T1, T2, T3, T4>((input, executable) => middleware(input, executable.Execute)));
  }

  /// <summary>
  /// Starts a pipeline from a step that receives downstream parameterless function.
  /// </summary>
  /// <typeparam name="T">Output type returned by the downstream function.</typeparam>
  /// <param name="middleware">Middleware function that can invoke downstream parameterless function.</param>
  /// <returns>Builder with downstream input fixed to <see cref="Unit" />.</returns>
  public static PipelineBuilder<T1, Unit, T, T4> Use<T>(Func<T1, Func<T>, T4> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new PipelineBuilder<T1, Unit, T, T4>(new AnonymousMiddleware<T1, Unit, T, T4>((input, executable) => {
      return middleware(input, () => executable.Execute(default));
    }));
  }

  /// <summary>
  /// Starts a pipeline from a step that forwards to downstream via typed <see cref="Action{T}" />.
  /// </summary>
  /// <typeparam name="T">Input type accepted by the downstream action.</typeparam>
  /// <param name="middleware">Middleware function that calls downstream action.</param>
  /// <returns>Builder whose downstream output type is <see cref="Unit" />.</returns>
  public static PipelineBuilder<T1, T, Unit, T4> Use<T>(Func<T1, Action<T>, T4> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new PipelineBuilder<T1, T, Unit, T4>(new AnonymousMiddleware<T1, T, Unit, T4>((input, executable) => {
      return middleware(input, i => executable.Execute(i));
    }));
  }

  /// <summary>
  /// Starts a pipeline from a step that forwards to downstream via parameterless <see cref="Action" />.
  /// </summary>
  /// <param name="middleware">Middleware function that calls downstream action.</param>
  /// <returns>Builder whose downstream input and output types are <see cref="Unit" />.</returns>
  public static PipelineBuilder<T1, Unit, Unit, T4> Use(Func<T1, Action, T4> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new PipelineBuilder<T1, Unit, Unit, T4>(new AnonymousMiddleware<T1, Unit, Unit, T4>((input, executable) => {
      return middleware(input, () => executable.Execute(default));
    }));
  }

}

/// <summary>
/// Entry point for building pipelines that return <see cref="Unit" />.
/// </summary>
/// <typeparam name="T">Input/output type of the final composed executable.</typeparam>
public static partial class Pipeline<T> {

  /// <summary>
  /// Starts a parameterless pipeline that returns value and can invoke downstream typed function.
  /// </summary>
  /// <typeparam name="T1">Input type expected by the downstream function.</typeparam>
  /// <typeparam name="T2">Output type returned by the downstream function.</typeparam>
  /// <param name="middleware">Middleware function that can invoke downstream function.</param>
  /// <returns>Builder for appending next middleware steps and terminal executable.</returns>
  public static PipelineBuilder<Unit, T1, T2, T> Use<T1, T2>(Func<Func<T1, T2>, T> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new PipelineBuilder<Unit, T1, T2, T>(new AnonymousMiddleware<Unit, T1, T2, T>((_, executable) => middleware(executable.Execute)));
  }

  /// <summary>
  /// Starts a parameterless pipeline that returns value and can invoke downstream parameterless function.
  /// </summary>
  /// <typeparam name="T1">Output type returned by the downstream parameterless function.</typeparam>
  /// <param name="middleware">Middleware function that can invoke downstream parameterless function.</param>
  /// <returns>Builder with downstream input fixed to <see cref="Unit" />.</returns>
  public static PipelineBuilder<Unit, Unit, T1, T> Use<T1>(Func<Func<T1>, T> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new PipelineBuilder<Unit, Unit, T1, T>(new AnonymousMiddleware<Unit, Unit, T1, T>((_, executable) => {
      return middleware(() => executable.Execute(default));
    }));
  }

  /// <summary>
  /// Starts a parameterless pipeline that returns value and can invoke downstream typed action.
  /// </summary>
  /// <typeparam name="T1">Input type accepted by the downstream action.</typeparam>
  /// <param name="middleware">Middleware function that can invoke downstream action.</param>
  /// <returns>Builder with downstream output fixed to <see cref="Unit" />.</returns>
  public static PipelineBuilder<Unit, T1, Unit, T> Use<T1>(Func<Action<T1>, T> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new PipelineBuilder<Unit, T1, Unit, T>(new AnonymousMiddleware<Unit, T1, Unit, T>((_, executable) => {
      return middleware(input => executable.Execute(input));
    }));
  }

  /// <summary>
  /// Starts a parameterless pipeline that returns value and can invoke downstream parameterless action.
  /// </summary>
  /// <param name="middleware">Middleware function that can invoke downstream parameterless action.</param>
  /// <returns>Builder with downstream input/output fixed to <see cref="Unit" />.</returns>
  public static PipelineBuilder<Unit, Unit, Unit, T> Use(Func<Action, T> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new PipelineBuilder<Unit, Unit, Unit, T>(new AnonymousMiddleware<Unit, Unit, Unit, T>((_, executable) => {
      return middleware(() => executable.Execute(default));
    }));
  }

  /// <summary>
  /// Starts a void pipeline from a step that receives downstream function.
  /// </summary>
  /// <typeparam name="T1">Input type expected by the downstream function.</typeparam>
  /// <typeparam name="T2">Output type returned by the downstream function.</typeparam>
  /// <param name="middleware">Middleware action that can invoke downstream function.</param>
  /// <returns>Builder for appending next middleware steps and terminal executable.</returns>
  public static PipelineBuilder<T, T1, T2, Unit> Use<T1, T2>(Action<T, Func<T1, T2>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new PipelineBuilder<T, T1, T2, Unit>(new AnonymousMiddleware<T, T1, T2, Unit>((input, executable) => {
      middleware(input, executable.Execute);
      return default;
    }));
  }

  /// <summary>
  /// Starts a void pipeline from a step that receives downstream parameterless function.
  /// </summary>
  /// <typeparam name="T1">Output type returned by the downstream parameterless function.</typeparam>
  /// <param name="middleware">Middleware action that can invoke downstream parameterless function.</param>
  /// <returns>Builder with downstream input fixed to <see cref="Unit" />.</returns>
  public static PipelineBuilder<T, Unit, T1, Unit> Use<T1>(Action<T, Func<T1>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new PipelineBuilder<T, Unit, T1, Unit>(new AnonymousMiddleware<T, Unit, T1, Unit>((input, executable) => {
      middleware(input, () => executable.Execute(default));
      return default;
    }));
  }

  /// <summary>
  /// Starts a void pipeline from a step that forwards via typed action.
  /// </summary>
  /// <typeparam name="T1">Input type accepted by the downstream action.</typeparam>
  /// <param name="middleware">Middleware action that calls downstream action.</param>
  /// <returns>Builder whose downstream output type is <see cref="Unit" />.</returns>
  public static PipelineBuilder<T, T1, Unit, Unit> Use<T1>(Action<T, Action<T1>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new PipelineBuilder<T, T1, Unit, Unit>(new AnonymousMiddleware<T, T1, Unit, Unit>((input, executable) => {
      middleware(input, i => executable.Execute(i));
      return default;
    }));
  }

  /// <summary>
  /// Starts a void pipeline from a step that forwards via parameterless action.
  /// </summary>
  /// <param name="middleware">Middleware action that calls downstream action.</param>
  /// <returns>Builder whose downstream input and output types are <see cref="Unit" />.</returns>
  public static PipelineBuilder<T, Unit, Unit, Unit> Use(Action<T, Action> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new PipelineBuilder<T, Unit, Unit, Unit>(new AnonymousMiddleware<T, Unit, Unit, Unit>((input, executable) => {
      middleware(input, () => executable.Execute(default));
      return default;
    }));
  }

}

/// <summary>
/// Entry point for building pipelines without input and output values.
/// </summary>
public static partial class Pipeline {

  /// <summary>
  /// Starts a parameterless void pipeline from a step that receives downstream typed function.
  /// </summary>
  /// <typeparam name="T1">Input type expected by the downstream function.</typeparam>
  /// <typeparam name="T2">Output type returned by the downstream function.</typeparam>
  /// <param name="middleware">Middleware action that can invoke downstream function.</param>
  /// <returns>Builder for appending next middleware steps and terminal executable.</returns>
  public static PipelineBuilder<Unit, T1, T2, Unit> Use<T1, T2>(Action<Func<T1, T2>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new PipelineBuilder<Unit, T1, T2, Unit>(new AnonymousMiddleware<Unit, T1, T2, Unit>((_, executable) => {
      middleware(executable.Execute);
      return default;
    }));
  }

  /// <summary>
  /// Starts a parameterless void pipeline from a step that receives downstream parameterless function.
  /// </summary>
  /// <typeparam name="T">Output type returned by the downstream parameterless function.</typeparam>
  /// <param name="middleware">Middleware action that can invoke downstream parameterless function.</param>
  /// <returns>Builder with downstream input fixed to <see cref="Unit" />.</returns>
  public static PipelineBuilder<Unit, Unit, T, Unit> Use<T>(Action<Func<T>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new PipelineBuilder<Unit, Unit, T, Unit>(new AnonymousMiddleware<Unit, Unit, T, Unit>((_, executable) => {
      middleware(() => executable.Execute(default));
      return default;
    }));
  }

  /// <summary>
  /// Starts a parameterless void pipeline from a step that receives downstream typed action.
  /// </summary>
  /// <typeparam name="T">Input type accepted by the downstream action.</typeparam>
  /// <param name="middleware">Middleware action that can invoke downstream action.</param>
  /// <returns>Builder with downstream output fixed to <see cref="Unit" />.</returns>
  public static PipelineBuilder<Unit, T, Unit, Unit> Use<T>(Action<Action<T>> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new PipelineBuilder<Unit, T, Unit, Unit>(new AnonymousMiddleware<Unit, T, Unit, Unit>((_, executable) => {
      middleware(i => executable.Execute(i));
      return default;
    }));
  }

  /// <summary>
  /// Starts a parameterless pipeline from a step that receives next action.
  /// </summary>
  /// <param name="middleware">Middleware action that calls downstream action.</param>
  /// <returns>Builder whose all generic types are <see cref="Unit" />.</returns>
  public static PipelineBuilder<Unit, Unit, Unit, Unit> Use(Action<Action> middleware) {
    ExceptionsHelper.ThrowIfNull(middleware, nameof(middleware));
    return new PipelineBuilder<Unit, Unit, Unit, Unit>(new AnonymousMiddleware<Unit, Unit, Unit, Unit>((_, executable) => {
      middleware(() => executable.Execute(default));
      return default;
    }));
  }

}