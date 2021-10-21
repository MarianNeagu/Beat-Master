using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiate_Lasers : MonoBehaviour
{
    public GameObject laser_L;
    public GameObject laser_R;
    public AudioManager audioManager;
    //public AudioSource laserStartSFX;
    //public AudioSource laserLoopSFX;
    public float inactiveTime;
    public float laserLoopSoundDelay;
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        laser_L.SetActive(false);
        laser_R.SetActive(false);
        Invoke("Instantiate", inactiveTime);
    }

    void Instantiate()
    {
        laser_L.SetActive(true);
        laser_R.SetActive(true);
        if(!BallController2.canStart)
        {
            //laserStartSFX.Play();
            audioManager.Play("LaserStart");
            Invoke("LoopSFX", laserLoopSoundDelay);
        }
        
    }
    void LoopSFX()
    {
        if (!BallController2.canStart)
            audioManager.Play("LaserLoop");
            //laserLoopSFX.Play();
            
    }
    
}
