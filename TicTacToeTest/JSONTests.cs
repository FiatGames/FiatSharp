using System;
using FiatSharp;
using FiatSharp.JsonConverters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace TicTacToeTest
{
    public class Foo { public string Bar { get; set; } }
    [TestClass]
    public class JSONTests
    {
        [TestMethod]
        public void InitialGameStateResultTest()
        {
            Foo f = new Foo {Bar = "a"};
            DateTime dt = new DateTime(2018, 5, 1, 14, 37, 0);
            string error = "{\"Left\":\"blah\"}";
            string gameState =
                "{\"_gameStateStage\":\"SettingUp\",\"_gameStateState\":{\"Bar\":\"a\"},\"_gameStateFutureMove\":{\"_futureMoveMove\":{\"Bar\":\"a\"},\"_futureMoveTime\":\"2018-05-01T18:37:00Z\"}}";
            string initial = $"{{\"Right\":[{{\"Bar\":\"a\"}},{gameState}]}}";

            Assert.AreEqual(initial,
                JsonConvert.SerializeObject(new InitialGameStateResult<Foo, Foo, Foo>
                {
                    InitialGameState = new Tuple<Foo, GameState<Foo, Foo>>(f, new GameState<Foo, Foo>
                    {
                        FutureMove = new FutureMove<Foo> {DateTime = dt, Move = f},
                        State = f,
                        Stage = GameStage.SettingUp
                    })
                },
                new InitialGameStateResultConverter<Foo, Foo, Foo>()));
            Assert.AreEqual(error,
                JsonConvert.SerializeObject(new InitialGameStateResult<Foo, Foo, Foo> {Error = "blah"},
                    new InitialGameStateResultConverter<Foo, Foo, Foo>()));
        }

        [TestMethod]
        public void NullTuple()
        {
            Tuple<string, string> t = new Tuple<string, string>(null, "a");
            string res = "[null,\"a\"]";

            Assert.AreEqual(res, JsonConvert.SerializeObject(t, new TupleConverter<string,string>()));
        }

        [TestMethod]
        public void GameStateTest()
        {
            DateTime dt = new DateTime(2018, 5, 1, 14, 37, 0);
            string gameState =
                "{\"_gameStateStage\":\"SettingUp\",\"_gameStateState\":{\"Bar\":\"a\"},\"_gameStateFutureMove\":{\"_futureMoveMove\":{\"Bar\":\"a\"},\"_futureMoveTime\":\"2018-05-01T18:37:00Z\"}}";

            Assert.AreEqual(gameState,
                JsonConvert.SerializeObject(
                    new GameState<Foo, Foo>
                    {
                        FutureMove = new FutureMove<Foo> {DateTime = dt, Move = new Foo { Bar = "a"}},
                        State = new Foo { Bar = "a"},
                        Stage = GameStage.SettingUp
                    }, new GameStateConverter<Foo, Foo>()));

            Assert.AreEqual(dt,
                JsonConvert.DeserializeObject<GameState<Foo, Foo>>(gameState,
                    new GameStateConverter<Foo, Foo>()).FutureMove.DateTime);
            Assert.AreEqual("a",
                JsonConvert.DeserializeObject<GameState<Foo, Foo>>(gameState,
                    new GameStateConverter<Foo, Foo>()).State.Bar);
        }

        [TestMethod]
        public void FutureMoveTest()
        {
            DateTime dt = new DateTime(2018,5,1,14,37,0);
            string futureMove = "{\"_futureMoveMove\":{\"Bar\":\"a\"},\"_futureMoveTime\":\"2018-05-01T18:37:00Z\"}";

            Assert.AreEqual(futureMove,
                JsonConvert.SerializeObject(new FutureMove<Foo> {DateTime = dt, Move = new Foo { Bar = "a"}},
                    new FutureMoveConverter<Foo>()));
            Assert.AreEqual("a", JsonConvert.DeserializeObject<FutureMove<Foo>>(futureMove, new FutureMoveConverter<Foo>()).Move.Bar);
            Assert.AreEqual(dt, JsonConvert.DeserializeObject<FutureMove<Foo>>(futureMove, new FutureMoveConverter<Foo>()).DateTime);
        }

        [TestMethod]
        public void GameStageTest()
        {
            string settingUp = "\"SettingUp\"";
            string done = "\"Done\"";
            string playing = "\"Playing\"";

            Assert.AreEqual(settingUp,
                JsonConvert.SerializeObject(GameStage.SettingUp, new GameStageConverter()));

            Assert.AreEqual(GameStage.SettingUp, JsonConvert.DeserializeObject<GameStage>(settingUp, new GameStageConverter()));
        }

        [TestMethod]
        public void FiatPlayer()
        {
            string system = "{\"tag\":\"System\"}";
            string player = "{\"tag\":\"FiatPlayer\",\"contents\":123}";

            Assert.AreEqual(system,
                JsonConvert.SerializeObject(new FiatPlayer {Id = null}, new FiatPlayerConverter()));
            Assert.AreEqual(player,
                JsonConvert.SerializeObject(new FiatPlayer { Id = 123 }, new FiatPlayerConverter()));

            Assert.IsTrue(JsonConvert.DeserializeObject<FiatPlayer>(system, new FiatPlayerConverter()).IsSystem);
            Assert.AreEqual(123,JsonConvert.DeserializeObject<FiatPlayer>(player, new FiatPlayerConverter()).Id);
        }
    }
}
