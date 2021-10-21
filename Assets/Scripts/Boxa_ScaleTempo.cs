using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boxa_ScaleTempo : MonoBehaviour
{

    public Animator animation;
    public float animSpeed;
    public string animStateName;
    public float startDelay;
    public bool playOnStart;
    void Start()
    {
        //animation[animStateName].speed = animSpeed;
        animation.speed = animSpeed;
        if(playOnStart)
            Invoke("Boom", startDelay);
    }

    public void Boom()
    {
        animation.Play(animStateName);
    }

    
}
