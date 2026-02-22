using System.Diagnostics.Contracts;
using System.Globalization;
using Interactions.Core;

namespace Interactions.Transformation.Parsing;

public abstract class Parser<T> : Transformer<string, T> {

  public override T Transform(string input) {
    return Parse(input);
  }

  protected abstract T Parse(string input);

}

public static class Parser {

  [Pure]
  public static Parser<int> Integer(CultureInfo cultureInfo = null) {
    return cultureInfo == null ? IntParser.Instance : new IntParser(cultureInfo);
  }

  [Pure]
  public static Parser<double> Double(CultureInfo cultureInfo = null) {
    return cultureInfo == null ? DoubleParser.Instance : new DoubleParser(cultureInfo);
  }

  [Pure]
  public static Parser<float> Single(CultureInfo cultureInfo = null) {
    return cultureInfo == null ? SingleParser.Instance : new SingleParser(cultureInfo);
  }

  [Pure]
  public static Parser<TimeSpan> TimeSpan(CultureInfo cultureInfo = null) {
    return cultureInfo == null ? TimeParser.Instance : new TimeParser(cultureInfo);
  }

  [Pure]
  public static Parser<T> Enum<T>() where T : struct {
    return EnumParser<T>.Instance;
  }

  [Pure]
  public static Parser<T> FromMethod<T>(Func<string, T> parsing) {
    ExceptionsHelper.ThrowIfNull(parsing, nameof(parsing));
    return new AnonymousParser<T>(parsing);
  }

}