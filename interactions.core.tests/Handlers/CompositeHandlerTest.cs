using AutoFixture;
using Interactions.Core.Handlers;
using JetBrains.Annotations;

namespace Interactions.Core.Tests.Handlers;

[TestSubject(typeof(CompositeHandler<,,>))]
public class CompositeHandlerTest {

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

    var query = new Query<int, decimal>();
    using IDisposable handle = query.Handle(Handler
      .Create<int, Player>(id => storage.Get(id))
      .Then(player => player.data)
      .Then(data => data.money)
    );

    Assert.Equal(firstPlayerMoney, query.Execute(0));
    Assert.Equal(secondPlayerMoney, query.Execute(1));
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