using System;
using System.Collections;
using System.Collections.Generic;
using Games.Defenses;
using Games.Global;
using Games.Global.Entities;
using Games.Global.Weapons;
using Games.Players;
using Games.Transitions;
using Networking;
using Networking.Client;
using Networking.Client.Room;
using UnityEngine;
using Utils;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Games.Attacks {
  [Serializable]
  public class TransistionTest {
    public string transition;

    public TransistionTest(string transition) { this.transition = transition; }
  }
  public enum TypeData { Nothing, Group, Wall, Trap }

  public class InitAttackPhase : MonoBehaviour {
    public const int MAP_SIZE = 25;

    [SerializeField]
    private ObjectPooler objectPoolerDefense;

    [SerializeField]
    private ObjectsInScene objectsInScene;

    [SerializeField]
    private ScriptsExposer se;

    [SerializeField]
    private InitDefense initDefense;

    [SerializeField]
    private HoverDetector hoverDetector;

    private string[, ] tempMap1;
    private string[, ] tempMap2;

    private Monster monster2;

    private bool endOfGeneration = false;

    private string currentMap;

    public void GenerateArray() {
      tempMap1 = new string[MAP_SIZE, MAP_SIZE];
      tempMap2 = new string[MAP_SIZE, MAP_SIZE];

      for (int i = 0; i < MAP_SIZE; i++) {
        for (int j = 0; j < MAP_SIZE; j++) {
          int rand = Random.Range(1, 11) % 10;

          //                    tempMap1[i, j] = (rand == 0 ? (int)TypeData.Wall
          //                    : (int)TypeData.Nothing) + ":" + "" + ":" +
          //                    "[]"; tempMap2[i, j] = (rand == 0 ?
          //                    (int)TypeData.Wall : (int)TypeData.Nothing) +
          //                    ":" + "" + ":" + "[]";

          tempMap1[i, j] = (int) TypeData.Nothing + ":" + "" + ":" + "[]";
          tempMap2[i, j] = (int) TypeData.Nothing + ":" + "" + ":" + "[]";
        }
      }

      tempMap2[MAP_SIZE / 4, MAP_SIZE / 4] =
          (int) TypeData.Group + ":" + "1" + ":" + "[]";
      //            tempMap2[MAP_SIZE - 3, MAP_SIZE - 3] = (int)TypeData.Group +
      //            ":" + "1" + ":" + "[3]";
    }

    public string ClearMapString(string maps) {
      Debug.Log(maps);
      string header = "GRID\":\"{";
      int indexInit = maps.IndexOf(header);

      maps = maps.Substring(indexInit + header.Length);

      int footerPosition = maps.IndexOf("}\"}");

      maps = maps.Substring(0, footerPosition);

      return maps;
    }

    public(string, int, int, TypeData, int, List<int>)
        ParseString(string maps) {
      TypeData type;
      int idElement;
      List<int> idEquipements = new List<int>();

      int x;
      int y;

      int lastIndexString = maps.IndexOf(";");

      string lineToParse = maps.Substring(0, lastIndexString);
      string returningMap = maps.Substring(lastIndexString + 1);

      int firstColon = lineToParse.IndexOf(':');
      int secondColon = lineToParse.IndexOf(':', firstColon + 1);

      x = Int32.Parse(lineToParse.Substring(0, firstColon).Trim());
      y = Int32.Parse(
          lineToParse.Substring(firstColon + 1, secondColon - (firstColon + 1))
              .Trim());

      lineToParse = lineToParse.Substring(secondColon + 1);
      firstColon = lineToParse.IndexOf(':');

      if (firstColon == -1) {
        firstColon = lineToParse.Length;
      } else {
        secondColon = lineToParse.IndexOf(':', firstColon + 1);
      }

      type =
          (TypeData) Int32.Parse(lineToParse.Substring(0, firstColon).Trim());

      Debug.Log("Want to spawn : " + type +
                " idType : " + lineToParse.Substring(0, firstColon).Trim());

      if (firstColon == lineToParse.Length) {
        return (returningMap, x, y, type, 0, idEquipements);
      }

      string idElementString =
          lineToParse.Substring(firstColon + 1, secondColon - (firstColon + 1))
              .Trim();
      idElement = Int32.Parse(idElementString);

      int firstBracket = lineToParse.IndexOf('[');
      int secondBracket = lineToParse.IndexOf(']');

      if (firstBracket == secondBracket - 1 || firstBracket == -1) {
        return (returningMap, x, y, type, idElement, idEquipements);
      }

      string lineArray = lineToParse.Substring(firstBracket + 1);
      string valueToParse = lineArray.Trim();

      bool wantToLeave = false;
      do {
        int indexComma = valueToParse.IndexOf(',');
        if (indexComma == -1) {
          indexComma = valueToParse.Length - 1;
          wantToLeave = true;
        }

        idEquipements.Add(Int32.Parse(valueToParse.Substring(0, indexComma)));

        if (!wantToLeave) {
          valueToParse = valueToParse.Substring(indexComma + 1);
        }
      } while (!wantToLeave);

      return (returningMap, x, y, type, idElement, idEquipements);
    }

    private void GeneratingMap(string maps, int playerIndex) {
      TypeData type;
      int idElement;
      List<int> idEquipements;
      int x;
      int y;

      maps = ClearMapString(maps);
      bool keyInMap = false;
      while (maps.Length > 0) {
        (maps, x, y, type, idElement, idEquipements) = ParseString(maps);

        Debug.Log("New map : " + maps);
        switch (type) {
        case TypeData.Nothing:
          break;
        case TypeData.Group:
          Vector3 newPos = new Vector3(
              x * 2 + initDefense.currentMap.transform.localPosition.x, 1.5f,
              y * 2 + initDefense.currentMap.transform.localPosition.z);
          GroupsMonster groups =
              DataObject.MonsterList.GetGroupsMonsterById(idElement);
          if (!keyInMap) {
            keyInMap = InstantiateGroupsMonster(groups, newPos, idEquipements);
          } else {
            InstantiateGroupsMonster(groups, newPos, idEquipements);
          }
          break;
        case TypeData.Trap:
          GameObject trap = objectPoolerDefense.GetPooledObject(0);
          TrapBehavior trapBehavior = trap.GetComponent<TrapBehavior>();
          trapBehavior
              .trapModels [idElement]
              .SetActive(true);
          trapBehavior.rotation = idEquipements[0];
          trap.transform.eulerAngles = new Vector3(
              trap.transform.eulerAngles.x, trapBehavior.rotation * 90,
              trap.transform.eulerAngles.z);
          trap.transform.position = new Vector3(
              x * 2 + initDefense.currentMap.transform.localPosition.x, 0.6f,
              y * 2 + initDefense.currentMap.transform.localPosition.z);
          trap.SetActive(true);

          DataObject.objectInScene.Add(trap);
          break;
        case TypeData.Wall:
          GameObject wall = objectPoolerDefense.GetPooledObject(1);
          wall.transform.position = new Vector3(
              x * 2 + initDefense.currentMap.transform.localPosition.x, 0,
              y * 2 + initDefense.currentMap.transform.localPosition.z);
          wall.SetActive(true);

          DataObject.objectInScene.Add(wall);
          break;
        }
      }

      if (!keyInMap) {
        KeyBehavior keyBehavior =
            initDefense.currentMap.GetComponent<MapStats>()
                .keyParent.GetComponent<KeyBehavior>();
        keyBehavior.doorPivot.SetActive(false);
        keyBehavior.endCube.SetActive(true);
      }
    }

    private void DesactiveDefenseMap() {
      if (hoverDetector.objectInHand != null) {
        hoverDetector.objectInHand.SetActive(false);
      }

      foreach (GameObject go in initDefense.gridCellList) {
        GridTileController gridTileController =
            go.GetComponent<GridTileController>();

        if (gridTileController.content != null) {
          gridTileController.content.transform.position =
              new Vector3(0, -10, 0);
          gridTileController.content.SetActive(false);
        }

        go.SetActive(false);
      }
    }

    public void StartAttackPhase() {
      DesactiveDefenseMap();

      objectsInScene.containerDefense.SetActive(false);
      objectsInScene.containerAttack.SetActive(true);

      DataObject.playerInScene.Clear();
      DataObject.monsterInScene.Clear();
      DataObject.objectInScene.Clear();

      if (!se.gameController.byPassDefense) {
        GeneratingMap(GameController.mapReceived, GameController.PlayerIndex);
      } else {
        string map = "GRID\":\"{-7:0:1:1:[0,0,0,0,0];}\"}";
        GeneratingMap(map, GameController.PlayerIndex);
      }

      GameController.mapReceived = null;

      TowersWebSocket.TowerSender("SELF", NetworkingController.CurrentRoomToken,
                                  "null", "setAttackReady", "null");
      StartCoroutine(WaitingForAttackPhase());
    }

    public IEnumerator ActivePlayer() {
      objectsInScene.mainCamera.SetActive(false);

      objectsInScene.playerPrefab[GameController.PlayerIndex]
          .playerGameObject.SetActive(true);
      objectsInScene.playerPrefab[GameController.PlayerIndex].canMove = true;

      objectsInScene.playerPrefab[GameController.PlayerIndex]
          .cameraGameObject.SetActive(true);

      Cursor.lockState = CursorLockMode.Locked;

      DataObject.playerInScene.Add(
          GameController.PlayerIndex,
          objectsInScene.playerPrefab[GameController.PlayerIndex]);

      while (CurrentRoom.loadGameAttack) {
        int nbMin = TransitionMenuGame.timerAttack / 60;
        int nbSec = TransitionMenuGame.timerAttack % 60;
        if (DataObject.playerInScene.Count > 0) {
          DataObject.playerInScene[GameController.PlayerIndex]
              .timerAttack.text =
              "Timer : " + nbMin + (nbMin > 0 ? "min" : "") + nbSec;
        }

        yield return new WaitForSeconds(0.5f);
      }
    }

    public IEnumerator WaitingForAttackPhase() {
      while (!CurrentRoom.loadGameAttack) {
        yield return new WaitForSeconds(0.5f);
      }

      StartCoroutine(ActivePlayer());

      endOfGeneration = true;
    }

    public bool InstantiateGroupsMonster(GroupsMonster groups, Vector3 position,
                                         List<int> equipment) {
      InstantiateParameters param;
      Monster monster;
      int nbMonsterInit = 0;
      bool shouldPutKey = false;
      bool hasAKey = false;
      Vector3 origPos = position;
      if (equipment[5] != 0) {
        shouldPutKey = true;
      }
      foreach (KeyValuePair<int, int> mobs in groups.monsterInGroups) {
        for (int i = 0; i < mobs.Value; i++) {
          monster = DataObject.MonsterList.GetMonsterById(mobs.Key);

          GameObject monsterGameObject = Instantiate(monster.model);
          monsterGameObject.transform.position = position;

          monster.IdEntity = DataObject.nbEntityInScene;
          DataObject.nbEntityInScene++;
          nbMonsterInit++;

          MonsterPrefab monsterPrefab =
              monsterGameObject.GetComponent<MonsterPrefab>();
          monster.InitMonster(monsterPrefab);

          groups.InitSpecificEquipment(monster, equipment);
          if (shouldPutKey) {
            monster.InitKey(
                initDefense.maps[initDefense.currentLevel - 1]
                    .mapsInLevel[NetworkingController.CurrentRoomMapsLevel
                                     [initDefense.currentLevel - 1]]
                    .GetComponent<MapStats>()
                    .keyParent);
            hasAKey = true;
            shouldPutKey = false;
          }
          position = origPos + GroupsPosition.position[nbMonsterInit];

          DataObject.monsterInScene.Add(monster);
        }
      }

      return hasAKey;
    }
  }
}