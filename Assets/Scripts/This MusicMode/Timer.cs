using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.VFX;

public class Timer : MonoBehaviour
{

    public BallController2 player;
    [SerializeField]
    VisualEffect fireWorks;
    private float clipLength;
    private float playTime;

    public float fireworksDuration;
    private float time_passed;
    private bool start;
    void Start()
    {
        start = false;
        time_passed = 0;
        clipLength = FirebaseManager.source.clip.length;
        playTime = clipLength / 16f;
        //fireWorks.Stop();
        fireWorks.enabled = false;
        print(clipLength);
        print(playTime);
    }

    // Update is called once per frame
    void Update()
    {
       if(player._audio.time >= playTime)
        {
            if(fireWorks.isActiveAndEnabled == false)
         fireWorks.enabled = true;
            start = true;
            playTime *= 2;
            fireWorks.Play();
        }
       if(start)
        {
            if (time_passed >= fireworksDuration)
            {
                fireWorks.Stop();
                start = false;
                time_passed = 0;
                //fireWorks.enabled = false;
            }
            else
                time_passed += Time.deltaTime;
        }
    }
}
