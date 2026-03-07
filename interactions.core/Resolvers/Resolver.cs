namespace Interactions.Core.Resolvers;

/// <summary>
/// Resolves a single instance, typically once per lazy handler.
/// </summary>
/// <typeparam name="T">Resolved instance type.</typeparam>
public interface IResolver<out T> {

  /// <summary>
  /// Resolves an instance.
  /// </summary>
  T Resolve();

}