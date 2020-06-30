using Games.Attacks;
using Games.Global;
using UnityEngine;

namespace Games {
public class ScriptsExposer : MonoBehaviour {
  [SerializeField]
  public InitAttackPhase initAttackPhase;

  [SerializeField]
  public DictionaryManager dm;

  [SerializeField]
  public GameController gameController;
}
}
