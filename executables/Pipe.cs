namespace Executables;

/// <summary>
/// Represents a transformation from <typeparamref name="TIn"/> to <typeparamref name="TOut"/>.
/// </summary>
/// <typeparam name="TIn">Input value type.</typeparam>
/// <typeparam name="TOut">Output value type.</typeparam>
public interface IPipe<in TIn, out TOut> {

  /// <summary>
  /// Applies the transformation to the provided input value.
  /// </summary>
  /// <param name="input">Input value.</param>
  /// <returns>Transformed output value.</returns>
  TOut Apply(TIn input);

}