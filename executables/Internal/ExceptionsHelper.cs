using System.Runtime.CompilerServices;

namespace Executables.Internal;

internal static class ExceptionsHelper {

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static void ThrowIfNull<T>(T arg, string paramName) where T : class {
    if (arg == null)
      throw new ArgumentNullException(paramName);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static void ThrowIfNullOrEmpty<T>(T[] args, string paramName) {
    if (args == null)
      throw new ArgumentNullException(paramName);
    if (args.Length == 0)
      throw new ArgumentException(paramName);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static void ThrowIfNullReference<T>(this T @this) where T : class {
    if (@this == null)
      throw new NullReferenceException();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static void ThrowIfNullOrEmpty(string value, string paramName) {
    if (value == null)
      throw new ArgumentNullException(paramName);
    if (value.Length == 0)
      throw new ArgumentException(paramName);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static void ThrowIfLessOrEqual<TComparable>(TComparable comparable, TComparable other, string paramName)
    where TComparable : IComparable<TComparable> {
    if (comparable.CompareTo(other) <= 0)
      throw new ArgumentException(paramName);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static void ThrowIfOutOfRange<TComparable>(TComparable comparable, TComparable min, TComparable max, string paramName)
    where TComparable : IComparable<TComparable> {
    if (comparable.CompareTo(min) < 0 || comparable.CompareTo(max) >= 0)
      throw new ArgumentOutOfRangeException(paramName);
  }

}