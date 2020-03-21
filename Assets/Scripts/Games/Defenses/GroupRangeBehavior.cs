using System;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Defenses
{
    public class GroupRangeBehavior : MonoBehaviour
    {
        public List<GridTileController> tilesInRange;
        private void OnTriggerEnter(Collider other)
        {
            //other.gameObject.GetComponent<GridTileController>().isTooCloseFromAMob = true;
            tilesInRange.Add(other.gameObject.GetComponent<GridTileController>());
        }

        private void OnTriggerExit(Collider other)
        {
            //other.gameObject.GetComponent<GridTileController>().isTooCloseFromAMob = false;
            tilesInRange.Remove(other.gameObject.GetComponent<GridTileController>());
        }

        public void SetAllTilesTo(bool bparam)
        {
            foreach (var tile in tilesInRange)
            {
                tile.isTooCloseFromAMob = bparam;
            }
        }

        public bool CheckContentEmpty()
        {
            foreach (var tile in tilesInRange)
            {
                if (tile.contentType != GridTileController.TypeData.Empty)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
