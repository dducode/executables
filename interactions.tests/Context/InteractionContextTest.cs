using Interactions.Context;
using Interactions.Core;
using Interactions.Guards;
using Interactions.Operations;
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
    IExecutable<Unit, Unit> executable = Policy
      .Guard<Unit>(() => {
        IReadonlyContext context = InteractionContext.Current;
        output.WriteLine($"Context: {{{context:v}}}");
        return context.Get<FeatureFlags>().IsDebug || context.Get<User>().IsAdmin;
      }, "Access denied")
      .Apply(Executable.Identity());

    if (isDebug || userIsAdmin)
      executable.Execute(default, InitContext);
    else
      Assert.Throws<AccessDeniedException>(() => executable.Execute(default, InitContext));
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