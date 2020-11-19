using System;
using System.Collections.Generic;
using Games.Transitions;
using Networking;
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

        [SerializeField] 
        public GameGridController gameGridController;

        public int currentLevel = 0;
        
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
            if (GameController.currentGameGrid == null)
            {
                return;
            }

            hoverDetector.dest = gameGridController.endZone;
            hoverDetector.startPos = gameGridController.startZone;

            Generate(GameController.currentGameGrid.size);
            Vector3 pos = defenseCamera.transform.position;
            pos.x = (GameController.currentGameGrid.size / 2) * GameGridController.TileOffset;
            pos.z = (GameController.currentGameGrid.size / 2) * GameGridController.TileOffset;
            defenseUIController.enabled = true;

            currentLevel++;
        }

        private void Generate(int size)
        {
            gridCellList = new List<GameObject>();

            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    currentCell = Instantiate(gridCell, new Vector3( i * GameGridController.TileOffset,  3f, j * GameGridController.TileOffset), Quaternion.Euler(90,0,0));

                    GridTileController currentTileController = currentCell.GetComponent<GridTileController>();
                    currentTileController.coordinates.x = (i+1)/2;
                    currentTileController.coordinates.y = (j+1)/2;
                    gridCellList.Add(currentCell);
                }
            }
        }
    }
}