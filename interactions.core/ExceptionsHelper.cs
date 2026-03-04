using System.Runtime.CompilerServices;

namespace Interactions.Core;

internal static class ExceptionsHelper {

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static void ThrowIfNull<T>(T obj, string paramName) where T : class {
    if (obj == null)
      throw new ArgumentNullException(paramName);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static void ThrowIfNullReference<T>(T @this) where T : class {
    if (@this == null)
      throw new NullReferenceException();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static void ThrowIfNullOrEmpty(string value, string paramName) {
    if (string.IsNullOrEmpty(value))
      throw new ArgumentException(paramName);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static void ThrowIfLessOrEqualZero(int value, string paramName) {
    if (value <= 0)
      throw new ArgumentOutOfRangeException(paramName);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static void ThrowIfLessOrEqualZero(TimeSpan value, string paramName) {
    if (value <= TimeSpan.Zero)
      throw new ArgumentOutOfRangeException(paramName);
  }

}