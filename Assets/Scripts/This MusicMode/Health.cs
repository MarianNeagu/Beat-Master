using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;
using UnityEditor;

public class Health : MonoBehaviour
{
    public static int _Health;
    //public static int _Score;
    public static float coinsInLevel;
    public static bool extraLife;

    public static float last_tempo;
    public static float blocks_speed=0;
    public static float spawnTime=0;


    private BannerView bannerView;

    // Start is called before the first frame update
    void Start()
    {
        if (Screen.currentResolution.refreshRate <= 60)
            Application.targetFrameRate = Screen.currentResolution.refreshRate;
        else Application.targetFrameRate = 60;
        /*
        switch(Screen.currentResolution.refreshRate)
        {
            case 60:
                Application.targetFrameRate = 60;
                break;
            case 40:
                Application.targetFrameRate = 40;
                break;
                case 30

        }
        */
        MobileAds.Initialize(initStatus => { });
        last_tempo = 0;
        //_Health = 4; 

        //4 vieti-true /3 vieti-false
        //extraLife = true;
        this.RequestBanner();

    }

    public void RequestBanner()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
            string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the top of the screen.
        AdSize adaptiveSize =
                AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

        if (this.bannerView != null)
            this.bannerView.Destroy();

        this.bannerView = new BannerView(adUnitId, adaptiveSize, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build();
        this.bannerView.LoadAd(request);
        //this.bannerView.Show();
    }

    public void CalcSpeed(float tempo)
    {
        if(FirebaseManager.tempo != last_tempo)
        {
            float nr;
            blocks_speed = tempo;
            nr = 1.1f;
            while (blocks_speed <= 3.7f)
            {
                blocks_speed = tempo * nr;
                nr += 0.1f;
            }
            //print(blocks_speed);
        }
    }

    public void SpawnTime(float tempo)
    {
        if(FirebaseManager.tempo!=last_tempo)
        {
            float nr;
           spawnTime = tempo;
            nr = 1;
            while (spawnTime > .5f)
            {
                spawnTime = tempo / nr;
                nr += 1;
            }
            //print(spawnTime);
        }
    }    
}
