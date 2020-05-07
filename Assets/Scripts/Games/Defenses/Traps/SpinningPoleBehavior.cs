using UnityEngine;

namespace Games.Defenses.Traps
{
public class SpinningPoleBehavior : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate (0,0,60*Time.deltaTime);
    }
}
}
