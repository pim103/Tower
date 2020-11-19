using System;
using System.Collections.Generic;
using FullSerializer;
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
        
        public static void InitGameControllerNetwork(GameController gameController)
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

                        if (callbackMessage.callbackMessages.message == "WON")
                        {
                            gameController.EndGame(false);
                        }
                        if (callbackMessage.callbackMessages.message == "DEATH")
                        {
                            gameController.EndGame(true);
                        }
                        if (callbackMessage.callbackMessages.message == "LoadGame")
                        {
                            CurrentRoom.loadGame = true;
                        }
                        if (callbackMessage.callbackMessages.message == "setGameLoaded")
                        {
                            Debug.Log("En attente de l'adversaire");
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
            if (callbackMessage.callbackMessages.message == "StartDefense")
            {
                GameController.currentGameGrid = callbackMessage.callbackMessages.maps;
                GameController.currentGameGrid.DisplayGridData();
                                
                CurrentRoom.loadGameDefense = true;
            }

            if (callbackMessage.callbackMessages.message == "LoadAttackGrid")
            {
                CurrentRoom.generateAttackGrid = true;
            }
            if (callbackMessage.callbackMessages.message == "StartAttack")
            {
                CurrentRoom.loadGameAttack = true;
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
            dictionary.Add("classes", ChooseDeckAndClass.currentRoleIdentity.classe.ToString());
            dictionary.Add("weapon", ChooseDeckAndClass.currentWeaponIdentity.categoryWeapon.ToString());
            dictionary.Add("equipmentDeck", ChooseDeckAndClass.equipmentDeckId.ToString());
            dictionary.Add("monsterDeck", ChooseDeckAndClass.monsterDeckId.ToString());
            
            TowersWebSocket.TowerSender("SELF", NetworkingController.CurrentRoomToken,"null", "initGame", TowersWebSocket.FromDictToString(dictionary));
        }
        
        public static void SendGridData()
        {
            fsSerializer serializer = new fsSerializer();
            fsData data;

            serializer.TrySerialize(GameController.currentGameGrid.GetType(), GameController.currentGameGrid, out data);

            Dictionary<string, string> args = new Dictionary<string, string>();
            args.Add("gameGrid", fsJsonPrinter.CompressedJson(data));

            TowersWebSocket.TowerSender("SELF", NetworkingController.CurrentRoomToken,"null", "waitingForAttackGrid", TowersWebSocket.FromDictToString(args));
        }

        public static void SendSetAttackReady()
        {
            TowersWebSocket.TowerSender("SELF", NetworkingController.CurrentRoomToken,"null", "setAttackReady", "null");
        }
    }
}