using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class BallController2 : MonoBehaviour
{
    #region Variabile (nu,nu fac abuz)
    [SerializeField]
    private float _impulse ;
    Touch touch;
    public Rigidbody rb;
    //private Vector2 mouse;
     private int lastVelocity;
    public float speed;
   
    public static float restart_music=0;
    public AudioSource _audio;
    //public GameObject _block;
    public bool canShoot = false;
    public static bool canStart = false;
    //public bool canStart_;
    //public AudioSource blockCollisionSound;
    [Space]
    [Header("UI")] 
    public GameObject restartMenu;
    //public new AudioSource  audio;
    //private float music_time;
    public InGameMenus menus;
    public RewardSystem rewardSystem;

    //Laser
    public ParticleSystem laserParticleSys_L;
    public ParticleSystem laserParticleSys_R;
    public ParticleSystem destroy;
    private bool deathParticleEvent;
    //public AudioSource laserDeathSFX;

    public GameObject finishScreen;
    public TextMeshProUGUI coinsColected;
    public TextMeshPro coinsBlaster;
    public TextMeshProUGUI progress,record;
    public Animator CameraShake;
    public GameObject FireWorks;
    public HeartsSystem hearts;

    public AudioManager audioManager;
    #endregion


    private void Start()
    {
        // print(PlayerPrefs.GetFloat("Music") + " asta de la Music");
        deathParticleEvent = false;
        canStart = false;
        restart_music = 0;
        rb = gameObject.GetComponent<Rigidbody>();
        _audio = FirebaseManager.source;
        audioManager = FindObjectOfType<AudioManager>();
        //print(_audio.clip.length * Health.spawnTime) ;
        //print(_audio.clip.length);      
    }

    private void Update()
    {      
        coinsBlaster.text = (PlayerPrefs.GetInt("Coins", 0) - Health.coinsInLevel).ToString();
        if (_audio.time >= _audio.clip.length && Spawner2.playerDead == false )
        {
            Time.timeScale = 1;
            FireWorks.SetActive(true);
            CameraShake.SetTrigger("End");
            StartCoroutine(FinalScreen(10));
            //coinsColected.text = "Coins:" + Environment.NewLine + '+' + (PlayerPrefs.GetInt("Coins", 0) - Health.coinsInLevel).ToString();
            coinsColected.text = '+' + (PlayerPrefs.GetInt("Coins", 0) - Health.coinsInLevel).ToString();
            canStart = false;
            Spawner2.playerDead = true;
            //print((int)(_audio.time / _audio.clip.length * 100));
            //_audio.Stop();
            PlayerPrefs.SetInt(FirebaseManager.buttonName, 100);
            //Destroy(this.gameObject);
            //menus.restartMenu.SetActive(true);
        }

        #region TouchScreenControll
        if(!IsPointerOverUIObject())
            if (Input.touchCount > 0  && Time.timeScale<=1 && canStart==true)
            {
                
                for (int i = 0; i < Input.touchCount; i++)
                {
                    touch = Input.touches[i];
                    Camera.main.ScreenToWorldPoint(touch.position);
                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            if (touch.position.x > Screen.width / 2)
                                Moving(1);
                            else
                                Moving(-1);
                            break;
                        case TouchPhase.Stationary:
                            if (canShoot)
                            {
                                if (touch.position.x > Screen.width / 2)
                                    Moving(1);
                                else
                                if (touch.position.x < Screen.width / 2)
                                    Moving(-1);
                            }
                            break;
                        case TouchPhase.Moved:
                            if (canShoot)
                            {
                                if (touch.position.x > Screen.width / 2)
                                    Moving(1);
                                else
                                if (touch.position.x < Screen.width / 2)
                                    Moving(-1);
                            }
                            break;
                    }
                }
                         
                //Touch touch = Input.GetTouch(0);
                

            }
        #endregion

        #region KeyboardControll (Debugging only, might delete later)
        if (Input.GetKeyDown("a") && Time.timeScale <= 1 && canStart==true)
            Moving(-1);
        if (Input.GetKeyDown("d") && Time.timeScale <= 1 && canStart == true)       
            Moving(1);      
        #endregion

        #region Aici am oprit velocitatea sa nu mai faca numerele alea urat la RigidBody :)
        if (rb.velocity.magnitude <= 0.09)
            rb.velocity = new Vector3(0, 0, 0);
        #endregion

        if (transform.position.x <= -3f && !deathParticleEvent)
        {
            laserParticleSys_L.Play();
            deathParticleEvent = true;           
        }
            
        if (transform.position.x >= 3f && !deathParticleEvent)
        {
            laserParticleSys_R.Play();
            deathParticleEvent = true;            
        }
        if ((transform.position.x <= -3f || transform.position.x >= 3f) && !Spawner2.playerDead)
        {
            hearts.NoHearts();
            int prog = (int)(_audio.time / _audio.clip.length * 100);
            progress.text = "PROGRESS:" + Environment.NewLine + prog.ToString() + '%';
            if (prog > PlayerPrefs.GetInt(FirebaseManager.buttonName, 0))
                PlayerPrefs.SetInt(FirebaseManager.buttonName, prog);
            record.text = "Coins: " + "+" + (PlayerPrefs.GetInt("Coins", 0) - Health.coinsInLevel).ToString();

            //record.text = "RECORD:" + Environment.NewLine + PlayerPrefs.GetInt(FirebaseManager.buttonName, 0).ToString() + '%' ;
            _audio.Pause();      
            //laserDeathSFX.Play();
            audioManager.Play("LaserDeath");
            //rb.velocity = Vector3.zero;
            //PlayerPrefs.SetFloat("Pitch", _audio.pitch);//pitch-ul la care a ajuns

            if (PlayerPrefs.GetString("powerup_extra_life") == "bought")
            {
                PlayerPrefs.SetString("powerup_extra_life", "unbought");
                Health.extraLife = false;
            }
            if(PlayerPrefs.GetInt("Multiply") == 2)
            {
                PlayerPrefs.SetString("powerup_extra_life", "unbought");
                PlayerPrefs.SetInt("Multiply", 1);
            }
            
            if ( restart_music >=0)           
                PlayerPrefs.SetFloat("Music", _audio.time - restart_music);                          
            else          
                PlayerPrefs.SetFloat("Music", 0);
            _audio.Stop();   
            Health._Health = 0;
            Health.last_tempo = FirebaseManager.tempo;
            rb.drag = 100;
            GetComponent<MeshRenderer>().enabled = false;
            Spawner2.playerDead = true;
            StartCoroutine(DelayDeath(1f));
        
            //Destroy(this.gameObject);     
            
        }
        else if(Health._Health == 0 && !Spawner2.playerDead)
        {
            int prog = (int)(_audio.time / _audio.clip.length * 100);
            progress.text = "PROGRESS:" + Environment.NewLine + prog.ToString() + '%';
            if (prog > PlayerPrefs.GetInt(FirebaseManager.buttonName, 0))
                PlayerPrefs.SetInt(FirebaseManager.buttonName, prog);
            record.text = "Coins: " + "+" + (PlayerPrefs.GetInt("Coins", 0) - Health.coinsInLevel).ToString();

            //record.text = "RECORD:" + Environment.NewLine + PlayerPrefs.GetInt(FirebaseManager.buttonName, 0).ToString() + '%' ;
            _audio.Pause();
            //laserDeathSFX.Play();
            audioManager.Play("LaserDeath");
            //rb.velocity = Vector3.zero;
            //PlayerPrefs.SetFloat("Pitch", _audio.pitch);//pitch-ul la care a ajuns

            if (PlayerPrefs.GetString("powerup_extra_life") == "bought")
            {
                PlayerPrefs.SetString("powerup_extra_life", "unbought");
                Health.extraLife = false;
            }
            if (PlayerPrefs.GetInt("Multiply") == 2)
            {
                PlayerPrefs.SetString("powerup_extra_life", "unbought");
                PlayerPrefs.SetInt("Multiply", 1);
            }


            if (restart_music >= 0)
                PlayerPrefs.SetFloat("Music", _audio.time - restart_music);
            else
                PlayerPrefs.SetFloat("Music", 0);
            _audio.Stop();          
            Health.last_tempo = FirebaseManager.tempo;
            rb.isKinematic = true;
            GetComponent<MeshRenderer>().enabled = false;
            Spawner2.playerDead = true;
            destroy.Play();
        
            StartCoroutine(DelayDeath(1.1f));
        }
    }
    IEnumerator DelayDeath(float timer)
    {
        yield return new WaitForSeconds(timer);
        if (rewardSystem.rewardedAd.IsLoaded())
        {
            menus.DeathMenu.SetActive(true);
            menus.AdButton.SetBool("start", true);

            menus.AdTimer = true;
            menus.AdLifetime = 5f;
            menus.adLifetime = 5f;
            menus.stats_timer = 2.5f;
        }
        else
            menus.restartMenu.SetActive(true);
        Destroy(this.gameObject);
    }
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
    /*
    void OnDeath()
    {
        Debug.Log("On Death");
        if (rewardSystem.rewardedAd.IsLoaded())
        {
            menus.DeathMenu.SetActive(true);
            menus.AdButton.SetBool("start", true);

            menus.AdTimer = true;
            menus.AdLifetime = 5f;
            menus.adLifetime = 5f;
            menus.stats_timer = 2.5f;
        }
        else
            menus.restartMenu.SetActive(true);
        Health._Health = 0;
        Health.last_tempo = FirebaseManager.tempo;
        Destroy(this.gameObject);
    }
    */
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Block"))
        {
            if(Time.timeScale != 1)
            {
                Time.timeScale = 1;
                _audio.pitch = 1;
            }
            //blockCollisionSound.Play();          
            Vector3 direction = Vector3.Reflect(new Vector3(lastVelocity, 0, 0), col.GetContact(0).normal);
            rb.velocity = direction * Mathf.Max(speed, 0);
            canShoot = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Shoot"))
            canShoot = true;
    }   

    private void Moving(int dir)
    {
        rb.AddForce(new Vector3(dir, 0, 0) * _impulse * Time.deltaTime, ForceMode.Impulse);
        lastVelocity = dir;
    }

    IEnumerator FinalScreen(float timer)
    {
        yield return new WaitForSeconds(timer);
        finishScreen.SetActive(true);
    }

    
}
