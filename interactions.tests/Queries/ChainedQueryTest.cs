using Interactions.Core.Queries;
using Interactions.Queries;
using JetBrains.Annotations;

namespace Interactions.Tests.Queries;

[TestSubject(typeof(ChainedQuery<,,>))]
public class ChainedQueryTest {

  [Theory]
  [InlineData("00:00:10", 10)]
  [InlineData("00:01:00", 60)]
  public void ConnectSecondToFirst(string expected, int value) {
    IQuery<int, TimeSpan> first = Executable
      .Create((int seconds) => TimeSpan.FromSeconds(seconds))
      .AsQuery();

    IQuery<TimeSpan, string> second = Executable
      .Create((TimeSpan time) => time.ToString())
      .AsQuery();

    IQuery<int, string> chained = first.Connect(second);
    Assert.Equal(expected, chained.Send(value));
  }

}