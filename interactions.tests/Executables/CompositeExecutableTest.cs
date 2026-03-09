using AutoFixture;
using Interactions.Core;
using Interactions.Executables;
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

    IExecutable<int, decimal> executable = Executable
      .Create<int, Player>(id => storage.Get(id))
      .Then(player => player.data)
      .Then(data => data.money);

    Assert.Equal(firstPlayerMoney, executable.Execute(0));
    Assert.Equal(secondPlayerMoney, executable.Execute(1));
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