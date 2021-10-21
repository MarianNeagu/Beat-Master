using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Storage;
using UnityEngine.Networking;
using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using TMPro;
using System.Net;
using UnityEngine.EventSystems;

public class FirebaseManager : MonoBehaviour
{
    public static AudioSource source;
    //string local_url = "file:///local/music/melody.mp3";
    //StorageReference gs_reference;
   public static float tempo;
    private string gs_reference;
    //FirebaseStorage storage = FirebaseStorage.DefaultInstance;
  public static string buttonName;
    //StorageReference space_ref;
    StorageReference https_reference;
    //public AudioClip audioClip;
    FirebaseStorage storage;
    public static bool canStart = false;
    DownloadHandlerAudioClip dlHandler;
    UnityWebRequest webRequest;
    MainMenuScript menu;

    public Health health;

    private void Start()
    {
        storage = FirebaseStorage.DefaultInstance;
        https_reference = storage.GetReferenceFromUrl("gs://ping-ball-3d.appspot.com/Music/AhrixNova.mp3");
        source = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if( SceneManager.GetActiveScene().name != "MusicMode2")
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                if (menu == null)
                    menu = GameObject.FindGameObjectWithTag("Spawner").GetComponent<MainMenuScript>();
                if (menu.connectionLost.activeSelf == false)
                {
                    menu.connectionLost.SetActive(true);
                    //menu.money.SetActive(false);
                }
                if (menu.loadingScreenCanvas.activeSelf == true)
                    menu.retryButton.SetActive(true);
                if(MainMenuScript.downloadProgress > 0)
                MainMenuScript.downloadProgress = 0;
            }
            else
               if ((Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork) && menu != null)
            {
                menu.connectionLost.SetActive(false);
                //menu.money.SetActive(true);
                menu.retryButton.SetActive(false);
                menu = null;
            }

            if (canStart)
            {
                StartCoroutine(SimpleRequest());
                canStart = false;
            }
            if (webRequest != null && webRequest.downloadProgress <= 1 && webRequest.downloadProgress >= 0 && (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork))
                MainMenuScript.downloadProgress = webRequest.downloadProgress;                         
        }
    }

    public void DownloadMelody()
    {

        // Fetch the download URL
        //buttonName = EventSystem.current.currentSelectedGameObject.name;
        https_reference.GetDownloadUrlAsync().ContinueWith((Task<Uri> task) =>
        {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                canStart = true;
                // ... now download the file via WWW or UnityWebRequest.
            }
            else
            {
                print("nu merge");
            }
        });

    }
    private IEnumerator SimpleRequest()
    {
        #region Switch butoane melodii
        switch (buttonName)
        {
            case "Ahrix - Nova":
                gs_reference = "https://firebasestorage.googleapis.com/v0/b/ping-ball-3d.appspot.com/o/Music%2FAhrixNova.mp3?alt=media&token=f17ddace-be02-47a0-9cab-2ce644f1a170";
                tempo = 128f;
                health.CalcSpeed(128f/60f);
                health.SpawnTime(128f/60f);
                break;
            case "Alan Walker - Fade":
                gs_reference = "https://firebasestorage.googleapis.com/v0/b/ping-ball-3d.appspot.com/o/Music%2FAlanFade.mp3?alt=media&token=9ff9d4d6-7522-428e-ac0f-95d41678d205";
                tempo = 180f;
                health.CalcSpeed(180f/60f);
                health.SpawnTime(180f/60f);
                break;
            case "Alan Walker - Spectre":
                gs_reference = "https://firebasestorage.googleapis.com/v0/b/ping-ball-3d.appspot.com/o/Music%2FAlanSpectre.mp3?alt=media&token=f3251f43-5f10-4d82-9380-e04d96ad9195";
                tempo = 128f;
                health.CalcSpeed(128f/60f);
                health.SpawnTime(128f/60f);
                break;
            case "Cartoon - On On feat Daniel Levi":
                gs_reference = "https://firebasestorage.googleapis.com/v0/b/ping-ball-3d.appspot.com/o/Music%2FDanielOnOn.mp3?alt=media&token=ce4cdd57-e4ba-4a73-8823-fa0c25766f35";
                tempo = 174f;
                health.CalcSpeed(174f/60f);
                health.SpawnTime(174f/60f);
                break;
            case "Different Heaven & EH!DE - My Heart":
                gs_reference = "https://firebasestorage.googleapis.com/v0/b/ping-ball-3d.appspot.com/o/Music%2FDiffHeavenMyHeart.mp3?alt=media&token=8243b118-4fbf-4ebe-8c4c-abf6150a5652";
                tempo = 174f;
                health.CalcSpeed(174f/60f);
                health.SpawnTime(174f/60f);
                break;
            case "Test":
                gs_reference = "https://firebasestorage.googleapis.com/v0/b/ping-ball-3d.appspot.com/o/Music%2F50%2051%2052%2053%2054%2055%2056%2057%2058%2059.mp3?alt=media&token=e5ef41b9-db41-4817-9c8b-5a1999206603";
                tempo = 100f;
                health.CalcSpeed(100f / 60f);
                health.SpawnTime(100f / 60f);
                break;
            default:
                //print("Eroare de atribuire a clipului. Verifica fiesierele sau MelodySelect.cs.");
                break;
        }
        #endregion

        webRequest = UnityWebRequestMultimedia.GetAudioClip(gs_reference, AudioType.MPEG);
        {  
            yield return webRequest.SendWebRequest();
            if (!webRequest.isNetworkError && !webRequest.isHttpError)
            {
               dlHandler = (DownloadHandlerAudioClip)webRequest.downloadHandler;
                if (dlHandler.isDone)
                {
                    //audioClip = DownloadHandlerAudioClip.GetContent(webRequest);
                    source.clip = dlHandler.audioClip;
                    if (source.clip != null)
                    {                    
                        PlayerPrefs.SetFloat("Music", 0);
                        Health.coinsInLevel = PlayerPrefs.GetInt("Coins", 0);
                        webRequest = null;
                        SceneManager.LoadScene("MusicMode2");                       
                    }
                    else
                    {
                        print("Retrieved AudioClip is null.");
                    }
                }
                else
                {
                    print("The download process is not completely finished.");
                }
            }
            else
            {
                if(menu.menus.activeInHierarchy == false)
                StartCoroutine(SimpleRequest());
            }
        }
    }
}
