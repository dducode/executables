using AutoFixture;
using Interactions.Core.Executables;
using JetBrains.Annotations;

namespace Interactions.Tests.Executables;

[TestSubject(typeof(CompositeExecutable<,,>))]
public class CompositeExecutableTest {

  [Fact]
  public void GetPlayerMoneyFromStorageTest() {
    var fixture = new Fixture();
    var firstPlayerMoney = fixture.Create<decimal>();
    var secondPlayerMoney = fixture.Create<decimal>();

    var storage = new PlayerStorage();
    storage.Add(new Player {
      id = 0,
      data = new PlayerData {
        money = firstPlayerMoney
      }
    });
    storage.Add(new Player {
      id = 1,
      data = new PlayerData {
        money = secondPlayerMoney
      }
    });

    IQuery<int, decimal> query = Executable
      .Create((int id) => storage.Get(id))
      .Then(player => player.data)
      .Then(data => data.money)
      .AsQuery();

    Assert.Equal(firstPlayerMoney, query.Send(0));
    Assert.Equal(secondPlayerMoney, query.Send(1));
  }

}

file class PlayerStorage {

  private readonly Dictionary<int, Player> _storage = new();

  public void Add(Player player) {
    _storage.Add(player.id, player);
  }

  public Player Get(int id) {
    return _storage[id];
  }

}

file class Player {

  public int id;
  public PlayerData data;

}

file struct PlayerData {

  public decimal money;

}