using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class InGameMenus : MonoBehaviour
{
    
    public GameObject restartMenu;
    public GameObject settingsMenuDeath, settingsMenuFinished, DeathMenu, finishedMenu, startPanel;
    public Animator AdButton;
    public Button stats_Button;
    public Button Ad;

    //public GameObject blackPanel;

    //public new AudioSource audio;
    public bool AdTimer = false;
    public float AdLifetime,adLifetime;
    public RewardSystem rewardSystem;
    public UnityEngine.UI.Image progress;
    //public TextMeshProUGUI _health;

    public TextMeshProUGUI stats;
    public float stats_timer;

    //Sound
    public AudioManager audioManager;
    private bool sfxPlayed;
    private void Awake()
    {
        startPanel.SetActive(true);
        finishedMenu.SetActive(false);
        restartMenu.SetActive(false);
        settingsMenuDeath.SetActive(false);
        settingsMenuFinished.SetActive(false);
        //settingsMenuPause.SetActive(false);
        DeathMenu.SetActive(false);
        stats_Button.interactable = false;
        //print(Time.timeScale);

    }
    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        stats_timer = -1;
        sfxPlayed = false;
        Time.timeScale = 1;
    }
    private void Update()
    {
        //_health.text = "Lives: " + Health._Health.ToString();
        //blackPanel.SetActive(false);

        if(DeathMenu.activeInHierarchy)
        {
            if (AdTimer)
            {
                if (AdLifetime > 0)
                {
                    if(!sfxPlayed)
                    {
                        sfxPlayed = true;
                        //tickingSFX.Play();
                        audioManager.Play("TickingClock");
                    }
                    AdLifetime -= Time.deltaTime;
                    progress.fillAmount = AdLifetime / adLifetime;
                }
                else
                {
                    AdTimer = false;
                    //AdButton.SetBool("InactiveAd", true);
                    AdButton.SetBool("start", false);
                    Ad.interactable = false;
                }
            }
            if (stats_timer >= 0)
                stats_timer -= Time.deltaTime;
            else if (stats_timer < 0 && stats_timer > -1)
            {
                stats_Button.interactable = true;
                stats.color = Color.white;
                stats_timer = -1;
            }
        }
    }
    

    
    

    #region Butoane RestartMenu

    public void TryAgain()
    {
        PlayerPrefs.SetFloat("Music", 0);
        //PlayerPrefs.SetFloat("Pitch", 1);
        Health.coinsInLevel = PlayerPrefs.GetInt("Coins", 0);
        Health._Health = 3;
        Health.extraLife = false;
        Time.timeScale = 1f;
        //Time.fixedDeltaTime = 0.02f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    #endregion

    #region Butoane Back
    


    public void BackToDeath()
    {
        restartMenu.SetActive(true);
        settingsMenuDeath.SetActive(false);
        DeathMenu.SetActive(false);
    }

    public void BackToFinished()
    {
        finishedMenu.SetActive(true);
        settingsMenuFinished.SetActive(false);
        DeathMenu.SetActive(false);
    }
    #endregion

    #region Butoane Universale - Settings, BackToMenu

    public void SettingsButtonDeath()
    {
        restartMenu.SetActive(false);
        settingsMenuDeath.SetActive(true);
        finishedMenu.SetActive(false);
    }
    public void SettingsButtonFinished()
    {
        restartMenu.SetActive(false);
        settingsMenuFinished.SetActive(true);
        finishedMenu.SetActive(false);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    #endregion

    
   

    public void InsertAd()
    {
        //print("There is an Ad for Durex or smth");
        //blackPanel.SetActive(true);
        //tickingSFX.Stop();
        audioManager.Stop("TickingClock");
        //Restart();
        rewardSystem.ShowAd();
        
        //print(PlayerPrefs.GetFloat("Music"));
    }

    public void Restart()
    {
        //tickingSFX.Stop();
        audioManager.Stop("TickingClock");
        DeathMenu.SetActive(false);
        restartMenu.SetActive(true);
        //BackFromRestart();    
    }
    
}
