namespace Executables;

/// <summary>
/// Represents a reversible transformation between two types.
/// </summary>
/// <typeparam name="T1">Source type of the forward transformation.</typeparam>
/// <typeparam name="T2">Source type of the backward transformation.</typeparam>
public interface IIso<T1, T2> {

  /// <summary>
  /// Converts a value from <typeparamref name="T1"/> to <typeparamref name="T2"/>.
  /// </summary>
  /// <param name="input">Source value.</param>
  /// <returns>Converted value.</returns>
  T2 Forward(T1 input);

  /// <summary>
  /// Converts a value from <typeparamref name="T2"/> back to <typeparamref name="T1"/>.
  /// </summary>
  /// <param name="input">Source value.</param>
  /// <returns>Converted value.</returns>
  T1 Backward(T2 input);

}