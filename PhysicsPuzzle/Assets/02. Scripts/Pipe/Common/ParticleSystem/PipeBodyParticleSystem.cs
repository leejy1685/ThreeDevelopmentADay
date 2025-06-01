using UnityEngine;

namespace _02._Scripts.Pipe.Common.ParticleSystem
{
    public class PipeBodyParticleSystem : MonoBehaviour
    {
        [SerializeField] private UnityEngine.ParticleSystem bodyParticle;

        public void BodyEffect(Color laserColor)
        {
            var main = bodyParticle.main;
            main.startColor = laserColor;

            bodyParticle.Stop();
            bodyParticle.Play();
        }

        public void StopBodyEffect()
        {
            bodyParticle.Stop();
        }
    }
}
