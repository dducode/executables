using Interactions.Core;
using Interactions.Core.Queries;
using Interactions.Extensions;
using Interactions.Guards;
using Interactions.Policies;
using Interactions.Validation;
using JetBrains.Annotations;

namespace Interactions.Tests.Policies;

[TestSubject(typeof(Policy<,>))]
public class PolicyTest {

  [Fact]
  public void PolicyQueryTest() {
    var query = new Query<int, string>();
    ManualGuard guard = Guard.Manual("Access denied - you cannot execute this query");

    IQuery<int, string> policyQuery = query.WithPolicy(Policy<int, string>
      .Validate(Validator.MoreThanOrEqual(0), Validator.NotEmptyString)
      .Guard(guard)
    );

    query.Handle(Handler
      .FromMethod((int seconds) => TimeSpan.FromSeconds(seconds))
      .Next(time => time.ToString())
    );

    Assert.Equal("00:00:10", policyQuery.Send(10));
    Assert.Throws<InvalidInputException>(() => policyQuery.Send(-1));
    guard.Open();
    Assert.Throws<AccessDeniedException>(() => policyQuery.Send(10));
    Assert.Throws<AccessDeniedException>(() => policyQuery.Send(-1));
    guard.Close();
    Assert.Equal("00:00:00", policyQuery.Send(0));
  }

}