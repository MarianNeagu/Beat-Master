using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.UI;
using TMPro;

public class RewardMenu : MonoBehaviour
{
    public RewardedAd rewardedAd;
    public GameObject rewardSeen;
    private bool getReward=false;
    public TextMeshProUGUI score;
    public AudioManager audioManager;
    public GameObject casetaValidare;
    public GameObject casetaBaniInsuf;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        this.CreateAndLoadRewardedAd();
        casetaValidare.SetActive(false);
    }
    private void Update()
    {
        if (getReward)
        {
            StartCoroutine(DelayUi());           
            getReward = false;
        }
    }
    IEnumerator DelayUi()
    {
        yield return new WaitForSeconds(0.2f);
        rewardSeen.SetActive(true);
    }
    public void CreateAndLoadRewardedAd()
    {
        string adUnitId = "ca-app-pub-3940256099942544/5224354917";
        this.rewardedAd = new RewardedAd(adUnitId);
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        
        AdRequest request = new AdRequest.Builder().Build();
        
        this.rewardedAd.LoadAd(request);
    }

    public void ShowValidationBox()
    {
        casetaValidare.SetActive(true);
    }

    public void CloseValidationBox()
    {
        casetaValidare.SetActive(false);
    }
    public void ShowAd()
    {
        casetaValidare.SetActive(false);
        casetaBaniInsuf.SetActive(false);
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }
    }

    public void claim()
    {
        audioManager.Play("Buy");
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) + 100);
        score.text = PlayerPrefs.GetInt("Coins", 0).ToString();
        rewardSeen.SetActive(false);
    }


    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        if (GetComponent<Image>().enabled == false)
            GetComponent<Image>().enabled = true;
    }



    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        GetComponent<Image>().enabled = false;
        //RequestReward();
        this.CreateAndLoadRewardedAd();
        
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        getReward = true;
       
    }


}
