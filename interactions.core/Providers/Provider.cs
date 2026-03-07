namespace Interactions.Core.Providers;

/// <summary>
/// Produces a new instance per call and may hold disposable resources.
/// </summary>
/// <typeparam name="T">Provided instance type.</typeparam>
public interface IProvider<out T> : IDisposable {

  /// <summary>
  /// Produces a new instance.
  /// </summary>
  T Get();

}