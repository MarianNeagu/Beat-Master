using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoostScript : MonoBehaviour
{
    public Button boost;
    public float timer;
    private float t=-1;
    public GameObject player;
    //public AudioSource laserLoopSFX;
    public Button startButton;
    public GameObject panel;
    public Animator anim;
    public GameObject GodMode;
    public ParticleSystem fum_minge;
    public AudioManager audioManager;
    public void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }
    void Update()
    {
        if (FirebaseManager.source.time >= FirebaseManager.source.clip.length)
            this.enabled = false;
        if (player == null && boost.IsActive())
            boost.gameObject.SetActive(false);
        if (t > 0)
        {
            t -= Time.deltaTime;
            boost.GetComponent<Image>().fillAmount = t / timer;
        }
        else if (t <= 0 && t>-1)
        {
            boost.gameObject.SetActive(false);
            StartCoroutine(Delay());
            
            t = -1;
        }
    }

    public void Boost()
    {
        PlayerPrefs.SetString("powerup_boost", "unbought");
        Time.timeScale = 2;
        FirebaseManager.source.pitch = 2;
        t = timer;
        boost.interactable = false;
        if(fum_minge.gameObject.activeSelf == true)
        fum_minge.gravityModifier = 1f;
    }
    IEnumerator Delay()
    {
        Time.timeScale = 0.9f;
        FirebaseManager.source.pitch = 0.9f;
        if(fum_minge.gameObject.activeSelf == true)
        fum_minge.gravityModifier = -0.05f;
        yield return new WaitForSeconds(3);
        Time.timeScale = 1f;
        FirebaseManager.source.pitch = 1f;
    }

    IEnumerator DisableBoost()
    {
        yield return new WaitForSeconds(5);
        if (boost.interactable)
        {
            //boost.interactable = false;
            boost.GetComponent<Animator>().SetBool("Fade", true);
        }
        
    }

    public void Starter()
    {

        PlayerPrefs.SetString("powerup_extra_life", "unbought");
        //laserLoopSFX.Stop();
        audioManager.Stop("LaserLoop");
        BallController2.canStart = true;
        FirebaseManager.source.Play();
        if (PlayerPrefs.GetString("powerup_boost") == "bought")
        {
            boost.gameObject.SetActive(true);
            StartCoroutine(DisableBoost());
        }
        panel.SetActive(false);
        anim.SetTrigger("Start");
        GodMode.SetActive(true);
    }


    public void God_mode()
    {
        Time.timeScale = 3f;
        FirebaseManager.source.pitch = 3f;
        GodMode.SetActive(false);
    }
}
