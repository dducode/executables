namespace Interactions.Internal.Extensions;

internal static class StackExtensions {

  internal static bool TryPop<T>(this Stack<T> stack, out T value) {
    if (stack.Count > 0) {
      value = stack.Pop();
      return true;
    }

    value = default;
    return false;
  }

}