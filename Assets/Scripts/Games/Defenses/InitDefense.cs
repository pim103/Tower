using System.Collections.Generic;
using Games.Transitions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Games.Defenses
{
    public class InitDefense : MonoBehaviour
    {
        [SerializeField] 
        private ScriptsExposer se;
        
        [SerializeField]
        private TransitionDefenseAttack transitionDefenseAttack;

        [SerializeField] 
        public DefenseUIController defenseUIController;
        
        [System.Serializable]
        public class MapsArrayClass
        {
            public GameObject[] mapsInLevel;
        }
        
        [SerializeField] 
        public MapsArrayClass[] maps;

        public int currentLevel = 0;
        public GameObject currentMap;
        public MapStats currentMapStats;
        
        [SerializeField] 
        private GameObject gridCell;
        private GameObject currentCell;

        [SerializeField] 
        private GameObject defenseCamera;
        
        [SerializeField]
        private HoverDetector hoverDetector;

        public List<GameObject> gridCellList;
        
        public void Init()
        {
            if (currentMap)
            {
                currentMap.SetActive(false);
            }

            if (currentLevel < maps.Length)
            {
                currentMap = maps[currentLevel].mapsInLevel[Random.Range(0, maps[currentLevel].mapsInLevel.Length)];
                currentMap.SetActive(true);
                currentMapStats = currentMap.GetComponent<MapStats>();
                hoverDetector.dest = currentMapStats.endCube;
                hoverDetector.startPos = currentMapStats.startPos;
                if (!se.gameController.byPassDefense)
                {
                    Generate();
                    defenseCamera.transform.position = currentMapStats.cameraPosition.transform.position;
                    defenseUIController.enabled = true;
                    transitionDefenseAttack.StartDefenseCounter();
                }
                else
                {
                    se.initAttackPhase.StartAttackPhase(null);
                }

                currentLevel++;
            }
            else
            {
                SceneManager.LoadScene("MenuScene");
            }
        }

        private void Generate()
        {
            gridCellList = new List<GameObject>();

            for (int i = currentMapStats.mapWidth*-1; i < currentMapStats.mapWidth; i+=2)
            {
                for (int j = currentMapStats.mapHeight*-1; j < currentMapStats.mapHeight; j+=2)
                {
                    currentCell = Instantiate(gridCell, new Vector3( i+currentMap.transform.localPosition.x+1,  3f, j+currentMap.transform.localPosition.z+1), Quaternion.Euler(90,0,0));
                    currentCell.transform.parent = currentMap.transform;
                    GridTileController currentTileController = currentCell.GetComponent<GridTileController>();
                    currentTileController.coordinates.x = (i+1)/2;
                    currentTileController.coordinates.y = (j+1)/2;
                    gridCellList.Add(currentCell);
                }
            }
        }
    }
}