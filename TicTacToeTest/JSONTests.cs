using System;
using FiatSharp;
using FiatSharp.JsonConverters;
using FiatSharp.Models;
using FiatSharp.Models.Http;
using FiatSharp.Models.Websocket;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace TicTacToeTest
{
    public class Foo { public string Bar { get; set; } }
    [TestClass]
    public class JSONTests
    {
        [TestMethod]
        public void ToServer()
        {
            ToServerCmd<Foo, Foo> c1 =
                new ToServerCmd<Foo, Foo>
                {
                    Player = new FiatPlayer {Id = 1},
                    Hash = new FiatGameHash{Hash = "123"},
                    Cmd = new MakeMove<Foo, Foo> { MoveToSubmit = new Foo { Bar = "a"} }
                };
            string a1 = JsonConvert.SerializeObject(c1, new ToServerCmdConverter<Foo, Foo>());
            ToServerCmd<Foo, Foo> c2 =
                new ToServerCmd<Foo, Foo>
                {
                    Player = new FiatPlayer { Id = 1 },
                    Hash = new FiatGameHash { Hash = "123" },
                    Cmd = new StartGame<Foo, Foo>()
                };
            string a2 = JsonConvert.SerializeObject(c2, new ToServerCmdConverter<Foo, Foo>());
            ToServerCmd<Foo, Foo> c3 =
                new ToServerCmd<Foo, Foo>
                {
                    Player = new FiatPlayer { Id = 1 },
                    Hash = new FiatGameHash { Hash = "123" },
                    Cmd = new UpdateSettings<Foo, Foo> { SettingsUpdate =  new Foo { Bar = "a"} }
                };
            string a3 = JsonConvert.SerializeObject(c3, new ToServerCmdConverter<Foo, Foo>());
        }

        [TestMethod]
        public void ToClient()
        {
            DateTime dt = new DateTime(2018, 5, 1, 14, 37, 0);
            string msg =
                @"{""tag"":""Msg"",""hash"":""47fdc26e-901a-4e4e-a945-ee0778525a22"",""settings"":{""Bar"":""a""},""state"":{""_gameStateStage"":""SettingUp"",""_gameStateState"":{""Bar"":""a""},""_gameStateFutureMove"":{""_futureMoveMove"":{""Bar"":""a""},""_futureMoveTime"":""2018-05-01T18:37:00Z""}}}";
            var a = JsonConvert.DeserializeObject<ToClientCmd<Foo, Foo, Foo>>(msg,new ToClientCmdConverter<Foo, Foo, Foo>());

            Assert.AreEqual(dt,a.Msg.NewGameState.FutureMove.DateTime);
            Assert.AreEqual("47fdc26e-901a-4e4e-a945-ee0778525a22", a.Msg.NewHash.Hash);
            Assert.AreEqual("a", a.Msg.NewGameState.State.Bar);
        }

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
