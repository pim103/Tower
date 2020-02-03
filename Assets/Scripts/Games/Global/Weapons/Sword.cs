using Scripts.Games.Global;
using PA_INST = Games.Global.Pattern.PatternInstructions;

namespace Games.Global.Weapons
{
    public class Sword : Weapon
    {
        //private Dictionary<PA_INST, int> pattern = new Dictionary<PA_INST, int>();

        private void Start()
        {
            pattern = new Pattern.Pattern[4];

            pattern[0] = new Pattern.Pattern(PA_INST.ROTATE_DOWN, 90, 1f, 0.01f);
            pattern[1] = new Pattern.Pattern(PA_INST.ROTATE_LEFT, 90, 1f, 0.01f);
            pattern[2] = new Pattern.Pattern(PA_INST.ROTATE_RIGHT, 90, 1f, 0.01f);
            pattern[3] = new Pattern.Pattern(PA_INST.ROTATE_UP, 90, 1f, 0.01f);
        }

        public override void BasicAttack()
        {

        }
    }
}
