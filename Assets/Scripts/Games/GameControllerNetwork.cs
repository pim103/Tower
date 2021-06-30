using System;
using System.Collections.Generic;
using FullSerializer;
using Games.Defenses;
using Games.Transitions;
using Networking;
using Networking.Client;
using Networking.Client.Room;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Games
{
    public static class GameControllerNetwork
    {
        private static CallbackMessages DeserializeMessage(string stringData)
        {
            fsSerializer serializer = new fsSerializer();
            fsData data;
            CallbackMessages callbackMessage = null;

            try
            {
                data = fsJsonParser.Parse(stringData);
                serializer.TryDeserialize(data, ref callbackMessage);
            }
            catch (Exception e)
            {
                Debug.Log("Can't read callback : " + e.Message);
            }

            if (callbackMessage == null)
            {
                return null;
            }
            
            return Tools.Clone(callbackMessage);
        }
        
        public static void InitGameControllerNetwork()
        {   
            if (TowersWebSocket.wsGame != null)
            {
                TowersWebSocket.wsGame.OnMessage += (sender, args) =>
                {
                    if (args.Data.Contains("callbackMessages"))
                    {
                        CallbackMessages callbackMessage = DeserializeMessage(args.Data);

                        if (callbackMessage == null)
                        {
                            return;
                        }

                        if (callbackMessage.callbackMessages.message != null)
                        {
                            Debug.Log(callbackMessage.callbackMessages.message);
                        }
                        
                        switch (callbackMessage.callbackMessages.message)
                        {
                            case "LoadGame":
                                CurrentRoom.loadGame = true;
                                break;
                            case "setGameLoaded":
                                Debug.Log("En attente de l'adversaire");
                                break;
                        }

                        if (CurrentRoom.loadGame)
                        {
                            CallbackPhaseManager(callbackMessage);
                        }
                    }
                };

                TowersWebSocket.wsGame.OnClose += (sender, args) =>
                {
                    if (args.Code != 1000)
                    {
                        NetworkingController.AuthToken = "";
                        NetworkingController.CurrentRoomToken = "";
                        NetworkingController.AuthRole = "";
                        NetworkingController.IsConnected = false;
                        NetworkingController.ConnectionClosed = args.Code;
                        NetworkingController.ConnectionStart = false;
                        SceneManager.LoadScene("MenuScene");
                    }
                };
            }
        }

        private static void CallbackPhaseManager(CallbackMessages callbackMessage)
        {
            if (callbackMessage.callbackMessages.roleTimer != -1)
            {
                TransitionMenuGame.waitingForStart = callbackMessage.callbackMessages.roleTimer;
                CurrentRoom.loadRoleAndDeck = true;
            }
            switch (callbackMessage.callbackMessages.message)
            {
                case "StartDefense":
                    //GameController.currentGameGrid = ;
                    CurrentRoom.loadGameDefense = true;
                    CurrentRoom.loadGameAttack = false;
                    break;
                case "LoadAttackGrid":
                    Debug.Log("Receive new grid");
                    GameController.currentGameGrid = callbackMessage.callbackMessages.maps;
                    CurrentRoom.generateAttackGrid = true;
                    GameController.currentGameGrid.DisplayGridData();
                    break;
                case "StartAttack":
                    CurrentRoom.loadGameAttack = true;
                    CurrentRoom.loadGameDefense = false;
                    CurrentRoom.generateAttackGrid = false;
                    break;
                case "WON":
                    GameController.SetEndOfGame(false);
                    break;
                case "DEATH":
                    GameController.SetEndOfGame(true);
                    break;
            }

            if (CurrentRoom.loadGameAttack)
            {
                if (callbackMessage.callbackMessages.attackTimer != -1)
                {
                    TransitionMenuGame.timerAttack = callbackMessage.callbackMessages.attackTimer;
                }
            }
                            
            if (CurrentRoom.loadGameDefense)
            {
                if (callbackMessage.callbackMessages.defenseTimer != -1)
                {
                    TransitionDefenseAttack.defenseTimer = callbackMessage.callbackMessages.defenseTimer;
                }
            }
        }

        public static void SendRoleAndClasses()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("classes", ChooseDeckAndClass.currentRoleIdentity.GetIdentityId().ToString());
            dictionary.Add("weapon", ChooseDeckAndClass.currentWeaponIdentity.GetIdentityId().ToString());
            dictionary.Add("equipmentDeck", ChooseDeckAndClass.equipmentDeckId.ToString());
            dictionary.Add("monsterDeck", ChooseDeckAndClass.monsterDeckId.ToString());
            
            TowersWebSocket.TowerSender("SELF", NetworkingController.CurrentRoomToken,"null", "initGame", TowersWebSocket.FromDictToString(dictionary));
        }
        
        public static void SendGridData(GameGrid grid)
        {
            fsSerializer serializer = new fsSerializer();
            fsData data;

            serializer.TrySerialize(grid.GetType(), grid, out data);

            Dictionary<string, string> args = new Dictionary<string, string>();
            args.Add("gameGrid", fsJsonPrinter.CompressedJson(data));

            TowersWebSocket.TowerSender("SELF", NetworkingController.CurrentRoomToken,"null", "waitingForAttackGrid", TowersWebSocket.FromDictToString(args, true));
        }

        public static void SendSetAttackReady()
        {
            TowersWebSocket.TowerSender("SELF", NetworkingController.CurrentRoomToken,"null", "setAttackReady", "null");
        }
    }
}