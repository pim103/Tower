using Games.Transitions;
using UnityEngine;

namespace Games.Defenses
{
    public class InitDefense : MonoBehaviour
    {
        [SerializeField] private ScriptsExposer se;
        
        [SerializeField]
        private TransitionDefenseAttack transitionDefenseAttack;

        [System.Serializable]
        public class MapsArrayClass
        {
            public GameObject[] mapsInLevel;
        }
        
        [SerializeField] 
        private MapsArrayClass[] maps;

        private int currentLevel = 0;
        public GameObject currentMap;
        private MapStats currentMapStats;
        
        [SerializeField] 
        private GameObject gridCell;
        private GameObject currentCell;

        [SerializeField] 
        private GameObject defenseCamera;
        
        public void Init()
        {
            currentMap = maps[currentLevel].mapsInLevel[Random.Range(0, maps[currentLevel].mapsInLevel.Length)];
            currentMap.SetActive(true);
            currentMapStats = currentMap.GetComponent<MapStats>();

            if (!se.gameController.byPassDefense)
            {
                Generate();
                defenseCamera.transform.position = currentMapStats.cameraPosition.transform.position;
                transitionDefenseAttack.StartDefenseCounter();
            }
            else
            {
                se.initAttackPhase.StartAttackPhase();
            }
        }

        private void Generate()
        {
            for (int i = currentMapStats.mapWidth*-1; i < currentMapStats.mapWidth; i+=2)
            {
                for (int j = currentMapStats.mapHeight*-1; j < currentMapStats.mapHeight; j+=2)
                {
                    currentCell = Instantiate(gridCell, new Vector3( i+currentMap.transform.localPosition.x+1,  3f, j+currentMap.transform.localPosition.z+1), Quaternion.Euler(90,0,0));
                    currentCell.transform.parent = currentMap.transform;
                }
            }
        }
    }
}