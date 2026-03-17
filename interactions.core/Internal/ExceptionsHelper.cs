using System.Runtime.CompilerServices;

namespace Interactions.Core.Internal;

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