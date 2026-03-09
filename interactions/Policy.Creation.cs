using Interactions.Policies;

namespace Interactions;

public static class Policy {

  public static PolicyBuilder<T1, T2> Of<T1, T2>() {
    return new PolicyBuilder<T1, T2>();
  }

  public static PolicyBuilder<T, T> Of<T>() {
    return new PolicyBuilder<T, T>();
  }

}