using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticles : MonoBehaviour
{
    // Start is called before the first frame update
    public ParticleSystem particles;

    public void Play()
    {
        particles.Play();
    }
}
