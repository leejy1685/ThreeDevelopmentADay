using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeBodyParticleSystem : MonoBehaviour
{
    [SerializeField] private ParticleSystem BodyParticle;

    public void BodyEffect(Color laserColor)
    {
        var main = BodyParticle.main;
        main.startColor = laserColor;

        BodyParticle.Stop();
        BodyParticle.Play();
    }

    public void StopBodyEffect()
    {
        BodyParticle.Stop();
    }
}
