namespace Interactions.Guards;

/// <summary>
/// Represents a guard that allows or denies handler execution.
/// </summary>
public abstract partial class Guard {

  /// <summary>
  /// Error message used when access is denied.
  /// </summary>
  public abstract string ErrorMessage { get; }

  /// <summary>
  /// Returns true when access is granted.
  /// </summary>
  public abstract bool TryGetAccess();

}