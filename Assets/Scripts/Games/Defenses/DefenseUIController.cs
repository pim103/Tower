using Games.Global.Weapons;
using Scripts.Games.Defenses;
using UnityEngine;
using UnityEngine.UI;

namespace Games.Defenses
{
    public class DefenseUIController : MonoBehaviour
    {
        [SerializeField] 
        private Button wallButton;

        [SerializeField] 
        public Text wallButtonText;
        
        [SerializeField] 
        private InitDefense initDefense;

        [SerializeField] 
        private ObjectPooler wallPooler;

        [SerializeField] 
        private HoverDetector hoverDetector;
        
        public int currentWallNumber;
        private int currentWallType;
        void OnEnable()
        {
            currentWallNumber = initDefense.currentMapStats.wallNumber;
            currentWallType = initDefense.currentMapStats.wallType;
            wallButtonText.text = "Mur x" + currentWallNumber;
            
            wallButton.onClick.AddListener(delegate
            {
                if (currentWallNumber > 0 && !hoverDetector.objectInHand)
                {
                    currentWallNumber -= 1;
                    wallButtonText.text = "Mur x" + currentWallNumber;
                    GameObject wall = wallPooler.GetPooledObject(currentWallType);
                    hoverDetector.objectInHand = wall;
                    wall.SetActive(true);
                }
            });
        }
        
        
    }
}
