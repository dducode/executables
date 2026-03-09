using Interactions.Policies;

namespace Interactions;

public static class AsyncPolicy {

  public static AsyncPolicyBuilder<T1, T2> Of<T1, T2>() {
    return new AsyncPolicyBuilder<T1, T2>();
  }

  public static AsyncPolicyBuilder<T, T> Of<T>() {
    return new AsyncPolicyBuilder<T, T>();
  }

}