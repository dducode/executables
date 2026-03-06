using Interactions.Core;
using Interactions.Guards;
using Interactions.Policies;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Interactions.Tests.Context;

[TestSubject(typeof(InteractionContext))]
public class InteractionContextTest(ITestOutputHelper output) {

  [Theory]
  [InlineData(true, true)]
  [InlineData(false, true)]
  [InlineData(true, false)]
  [InlineData(false, false)]
  public void SwitchGuardByFeatureFlags(bool isDebug, bool userIsAdmin) {
    Policy<Unit, Unit> guard = Policy<Unit, Unit>.Guard(() => {
      IReadonlyContext context = InteractionContext.Current;
      output.WriteLine($"Context: {{{context:v}}}");
      return context.Get<User>().IsAdmin;
    }, "Access denied");

    Policy<Unit, Unit> policy = Policy<Unit, Unit>.Optional(() => {
      IReadonlyContext context = InteractionContext.Current;
      output.WriteLine($"Context: {{{context:v}}}");
      return !context.Get<FeatureFlags>().IsDebug;
    }, guard);

    if (isDebug || userIsAdmin)
      policy.Execute(default, Executable.Identity(), InitContext);
    else
      Assert.Throws<AccessDeniedException>(() => policy.Execute(default, Executable.Identity(), InitContext));
    return;

    void InitContext(InteractionContext context) {
      context.Name = nameof(SwitchGuardByFeatureFlags);
      context.Set(new FeatureFlags { IsDebug = isDebug });
      context.Set(new User { IsAdmin = userIsAdmin });
    }
  }

  private class User {

    public bool IsAdmin { get; init; }

  }

  private class FeatureFlags {

    public bool IsDebug { get; init; }

  }

}