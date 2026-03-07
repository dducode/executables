using System.Diagnostics.Contracts;
using System.Text;
using Interactions.Core;
using Interactions.Extensions;

namespace Interactions.Transformation;

public abstract class Transformer<T1, T2> {

  public abstract T2 Transform(T1 input);

}

public static class Transformer {

  [Pure]
  public static Transformer<T, T> Identity<T>() {
    return IdentityTransformer<T>.Instance;
  }

  [Pure]
  public static Transformer<T1, T2> Dynamic<T1, T2>(IProvider<Transformer<T1, T2>> provider) {
    ExceptionsHelper.ThrowIfNull(provider, nameof(provider));
    return new DynamicTransformer<T1, T2>(provider);
  }

  [Pure]
  public static Transformer<T1, T2> Dynamic<T1, T2>(Func<Transformer<T1, T2>> provider) {
    return Dynamic(Provider.Create(provider));
  }

  [Pure]
  public static Transformer<T1, T2> Lazy<T1, T2>(IResolver<Transformer<T1, T2>> resolver) {
    ExceptionsHelper.ThrowIfNull(resolver, nameof(resolver));
    return new LazyTransformer<T1, T2>(resolver);
  }

  [Pure]
  public static Transformer<T1, T2> Lazy<T1, T2>(Func<Transformer<T1, T2>> resolver) {
    return Lazy(Resolver.Create(resolver));
  }

  [Pure]
  public static SymmetricTransformer<byte[], string> Encode(Encoding encoding = null) {
    return encoding == null ? Encoder.FromUTF8 : new Encoder(encoding);
  }

  [Pure]
  public static SymmetricTransformer<byte[], string> Base64Transformer() {
    return Transformation.Base64Transformer.Instance;
  }

  [Pure]
  public static Transformer<IEnumerable<T>, T> Aggregate<T>(Func<T, T, T> accumulate) {
    ExceptionsHelper.ThrowIfNull(accumulate, nameof(accumulate));
    return new Aggregator<T>(accumulate);
  }

  [Pure]
  public static Transformer<IEnumerable<T1>, T2> Aggregate<T1, T2>(Func<T2> seed, Func<T2, T1, T2> accumulate) {
    ExceptionsHelper.ThrowIfNull(seed, nameof(seed));
    ExceptionsHelper.ThrowIfNull(accumulate, nameof(accumulate));
    return new Aggregator<T1, T2>(seed, accumulate);
  }

  [Pure]
  public static Transformer<IEnumerable<T1>, IEnumerable<T2>> Select<T1, T2>(Func<T1, T2> selection) {
    ExceptionsHelper.ThrowIfNull(selection, nameof(selection));
    return new Selector<T1, T2>(selection);
  }

  [Pure]
  public static Transformer<IEnumerable<T1>, IEnumerable<T2>> SelectMany<T1, T2>(Func<T1, IEnumerable<T2>> selection) {
    ExceptionsHelper.ThrowIfNull(selection, nameof(selection));
    return new ManySelector<T1, T2>(selection);
  }

  [Pure]
  public static Transformer<IEnumerable<T>, T> First<T>(Func<T, bool> predicate = null) {
    return predicate == null ? FirstSelector<T>.Instance : new FirstSelector<T>(predicate);
  }

  [Pure]
  public static SymmetricTransformer<string, IEnumerable<string>> SplitConcat(string separator) {
    ExceptionsHelper.ThrowIfNull(separator, nameof(separator));
    return new SplitConcatStringsTransformer(separator.ToCharArray());
  }

  [Pure]
  public static SymmetricTransformer<IEnumerable<string>, string> ConcatSplit(string separator) {
    ExceptionsHelper.ThrowIfNull(separator, nameof(separator));
    return new SplitConcatStringsTransformer(separator.ToCharArray()).Inverse();
  }

  [Pure]
  public static Transformer<string, IEnumerable<string>> Split(string separator) {
    return SplitConcat(separator);
  }

  [Pure]
  public static Transformer<IEnumerable<string>, string> Concat(string separator) {
    return ConcatSplit(separator);
  }

  [Pure]
  public static SymmetricTransformer<IEnumerable<T>, List<T>> ToList<T>() {
    return ListTransformer<T>.Instance;
  }

  [Pure]
  public static SymmetricTransformer<IEnumerable<T>, T[]> ToArray<T>() {
    return ArrayTransformer<T>.Instance;
  }

  [Pure]
  public static Transformer<T1, T2> Create<T1, T2>(Func<T1, T2> transformation) {
    ExceptionsHelper.ThrowIfNull(transformation, nameof(transformation));
    return new AnonymousTransformer<T1, T2>(transformation);
  }

  [Pure]
  public static SymmetricTransformer<T1, T2> Create<T1, T2>(Func<T1, T2> forward, Func<T2, T1> backward) {
    ExceptionsHelper.ThrowIfNull(forward, nameof(forward));
    ExceptionsHelper.ThrowIfNull(backward, nameof(backward));
    return new AnonymousSymmetricTransformer<T1, T2>(forward, backward);
  }

}