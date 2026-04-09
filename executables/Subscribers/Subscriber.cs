namespace Executables.Subscribers;

/// <summary>
/// Represents a receiver that handles published event values.
/// </summary>
/// <typeparam name="T">Type of received event value.</typeparam>
public interface ISubscriber<in T> : IExecutable<T, Unit> {

  /// <summary>
  /// Receives a published value.
  /// </summary>
  /// <param name="input">Published value.</param>
  void Receive(T input);

}