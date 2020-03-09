using System.Collections;

namespace Games.Global
{
    public interface EffectInterface
    {
        void StartCoroutineEffect(Effect effect);
        IEnumerator PlayEffectOnTime(Effect effect);
        void StopCurrentEffect(Effect effect);
    }
}