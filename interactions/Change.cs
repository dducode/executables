namespace Interactions;

/// <summary>
/// Represents a transition from an old value to a new value.
/// </summary>
/// <typeparam name="T">Type of the changed value.</typeparam>
public readonly record struct Change<T>(T Old, T New) {

  /// <summary>
  /// Gets the previous value.
  /// </summary>
  public readonly T Old = Old;
  /// <summary>
  /// Gets the updated value.
  /// </summary>
  public readonly T New = New;

}