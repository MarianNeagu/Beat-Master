using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class ShopSystem : MonoBehaviour
{
    [Header("BALLS: ")]
    public GameObject[] ballMainObj;
    
    public Material[] ballMat;
    public bool[] hasParticleSys;
    public Color[] particleSysColor;
    [SerializeField] ParticleSystem[] particleSystemsBalls;
    public GameObject[] lacatBalls;
    public int[] priceBall;
    public GameObject[] selectionBorder_Balls;
    [Space]
    public GameObject casetaValidarePlataBall;
    

    [Space][Header("POWER-UPS:")]
    public GameObject[] powerupMainObj_SHOP;
    public GameObject[] powerupMainObj_PLAY;
    public GameObject[] lacatPowerups_SHOP;
    public GameObject[] lacatPowerups_PLAY;
    public int[] pricePowerup;
    public string[] powerupType;
    public GameObject[] selectionBorder_Powerups_SHOP;
    public GameObject[] selectionBorder_Powerups_PLAY;
    [Space]
    public GameObject casetaValidarePlataPowerup;

    [Space][Header("GENERALE:")]
    
    public GameObject casetaBaniInsuf;
    public bool inTimpulUneiPlati;
    public int currentIndexNumber;
    public TextMeshProUGUI coinText;

    [Space]
    [Header("SUNETE:")]
    public AudioManager audioManager;

    /*
     * EXISTA 2 TIPURI DE ITEME:
     * -TEMPORARE: PowerUps
     * -DEFINITIVE: Balls
     * 
     * Fiecare player prefs al bilei este de forma ball_{indexNum}, iar al powerup-urilor de forma powerup_{indexNum}
     * 
     * DEFINITIVE:
     * Acestea pot avea 3 tipuri de string uri:
     * -unbought -> asa este la inceput fiecare item, cu exceptia bilei default care este "selected"
     *           -> lacatul este activ
     * -bought -> asa va ramane fiecare item care a trecut prin sistemul de plata
     *         -> lacatul nu mai este activ
     * -selected -> un singur item din lista cu balls poate avea proprietatea, dar in lista cu powerups nu se aplica
     *           -> o bordura va fi plasata deasupra itemului 
     * 
     * Nota: un item poate fi "selected" doar daca este bought sau daca tocmai ce a fost facuta o plata
     */

    public void Start()
    {
        for(int i = 0; i < particleSystemsBalls.Length; i++)
            if(particleSystemsBalls[i] != null)
            particleSystemsBalls[i].startColor = particleSysColor[i];

        #region INITIALIZARI
        casetaBaniInsuf.SetActive(false);
        casetaValidarePlataBall.SetActive(false);
        inTimpulUneiPlati = false;
        #endregion
        audioManager = FindObjectOfType<AudioManager>();

        #region BALLS
        for (int i = 0; i < ballMainObj.Length; i++)
        {
            
            if (!PlayerPrefs.HasKey("ball_" + i)) //LA PRIMUL JOC EVER
                if (i != 0)
                {
                    PlayerPrefs.SetString("ball_" + i, "unbought");
                    lacatBalls[i].SetActive(true);
                    selectionBorder_Balls[i].GetComponent<CanvasRenderer>().cullTransparentMesh = true;
                }
                else
                {
                    lacatBalls[0].SetActive(false);
                    selectionBorder_Balls[0].GetComponent<CanvasRenderer>().cullTransparentMesh = false;
                    PlayerPrefs.SetString("ball_0", "selected");
                    AssignedParameters.ballMat = ballMat[0];
                    AssignedParameters.hasParticleSys = false;
                }
            // Dupa primul joc
            else if (PlayerPrefs.GetString("ball_" + i) == "selected")
            {
                AssignedParameters.ballMat = ballMat[i];
                if (hasParticleSys[i])
                {
                    AssignedParameters.particleSysColor = particleSysColor[i];
                    AssignedParameters.hasParticleSys = true;
                }
                else AssignedParameters.hasParticleSys = false;
                lacatBalls[i].SetActive(false);
                selectionBorder_Balls[i].GetComponent<CanvasRenderer>().cullTransparentMesh = false;
            }
            else if (PlayerPrefs.GetString("ball_" + i) == "bought")
            {
                lacatBalls[i].SetActive(false);
                selectionBorder_Balls[i].GetComponent<CanvasRenderer>().cullTransparentMesh = true;
            }
            else if (PlayerPrefs.GetString("ball_" + i) == "unbought")
            {
                lacatBalls[i].SetActive(true);
                selectionBorder_Balls[i].GetComponent<CanvasRenderer>().cullTransparentMesh = true;
                
            }
        }
        #endregion

        #region POWERUPS
        for (int i = 0; i < powerupMainObj_SHOP.Length; i++)
        {
            if (!PlayerPrefs.HasKey("powerup_" + powerupType[i]))
            {
                Debug.Log("Prima oara ever");
                PlayerPrefs.SetString("powerup_" + powerupType[i], "unbought");
                lacatPowerups_SHOP[i].SetActive(true);
                lacatPowerups_PLAY[i].SetActive(true);
                //selectionBorder_Powerups_SHOP[i].SetActive(false);
                //selectionBorder_Powerups_PLAY[i].SetActive(false);

                //Health initializari
                Health.extraLife = false;
                Health._Health = 3;
                PlayerPrefs.SetInt("Multiply", 1);
                //boost initializari

            }
            else if (PlayerPrefs.GetString("powerup_" + powerupType[i]) == "bought")
            {
                Debug.Log("alte dati - cumparat");
                lacatPowerups_SHOP[i].SetActive(false);
                lacatPowerups_PLAY[i].SetActive(false);
                //selectionBorder_Powerups_SHOP[i].SetActive(false);
                //selectionBorder_Powerups_PLAY[i].SetActive(true);
                switch (powerupType[i])
                {
                    case "extra_life":
                        Health.extraLife = true;
                        Health._Health = 4;
                        break;

                    case "boost":
                        break;

                    case "coin_multiplier":
                        PlayerPrefs.SetInt("Multiply", 2);
                        break;
                }
            }
            else if (PlayerPrefs.GetString("powerup_" + powerupType[i]) == "unbought")
            {
                Debug.Log("alte dati - necumparat");
                lacatPowerups_SHOP[i].SetActive(true);
                lacatPowerups_PLAY[i].SetActive(true);
                //selectionBorder_Powerups_SHOP[i].SetActive(false);
                //selectionBorder_Powerups_PLAY[i].SetActive(false);
                switch (powerupType[i])
                {
                    case "extra_life":
                        Health.extraLife = false;
                        Health._Health = 3;
                        break;

                    case "boost":
                        break;

                    case "coin_multiplier":
                        PlayerPrefs.SetInt("Multiply", 1);
                        break;
                }
            }
        }
        #endregion 


    }

    #region BALL
    public void RequestBuyOrSelectBall(int indexNumber) // se pune la fiecare buton in parte
    {
        
        if(!inTimpulUneiPlati)
        {
            //Se selecteaza bila
            if (PlayerPrefs.GetString("ball_" + indexNumber) == "bought")
            {
                //selectItem_Sound.Play();
                audioManager.Play("VirtualButton");

                PlayerPrefs.SetString("ball_" + indexNumber, "selected");
                selectionBorder_Balls[indexNumber].GetComponent<CanvasRenderer>().cullTransparentMesh = false;
                AssignedParameters.ballMat = ballMat[indexNumber];
                if (hasParticleSys[indexNumber])
                {
                    AssignedParameters.particleSysColor = particleSysColor[indexNumber];
                    AssignedParameters.hasParticleSys = true;
                } else AssignedParameters.hasParticleSys = false;
                for (int i = 0; i < ballMainObj.Length; i++)
                    if (indexNumber != i)
                    {
                        if(PlayerPrefs.GetString("ball_" + i) == "selected")
                            PlayerPrefs.SetString("ball_" + i, "bought"); // restul bought ca sa nu existe mai multe selected
                        selectionBorder_Balls[i].GetComponent<CanvasRenderer>().cullTransparentMesh = true;
                    }
            }

            //Se incearca cumpararea bilei
            else if (PlayerPrefs.GetString("ball_" + indexNumber) == "unbought") 
            {
                if (PlayerPrefs.GetInt("Coins") >= priceBall[indexNumber])
                {
                    //selectItem_Sound.Play();
                    audioManager.Play("VirtualButton");
                    inTimpulUneiPlati = true;
                    currentIndexNumber = indexNumber;
                    casetaValidarePlataBall.SetActive(true);
                    
                }
                //Caseta bani insuf.
                else
                {
                    //insufficientBalance_Sound.Play();
                    audioManager.Play("Error");
                    //inTimpulUneiPlati = true;
                    casetaBaniInsuf.SetActive(true);
                }
            }
        }    
    }

    public void PaymentValidationBall()
    {
        //buyItem_Sound.Play();
        audioManager.Play("Buy");
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - priceBall[currentIndexNumber]);
        coinText.text = PlayerPrefs.GetInt("Coins").ToString();

        PlayerPrefs.SetString("ball_" + currentIndexNumber, "selected");
        lacatBalls[currentIndexNumber].SetActive(false);
        inTimpulUneiPlati = false;
        casetaValidarePlataBall.SetActive(false);

        selectionBorder_Balls[currentIndexNumber].GetComponent<CanvasRenderer>().cullTransparentMesh = false;

        AssignedParameters.ballMat = ballMat[currentIndexNumber];
        if (hasParticleSys[currentIndexNumber])
        {
            AssignedParameters.particleSysColor = particleSysColor[currentIndexNumber];
            AssignedParameters.hasParticleSys = true;
        }else AssignedParameters.hasParticleSys = false;
        for (int i = 0; i < ballMainObj.Length; i++)
            if (currentIndexNumber != i)
            {
                if (PlayerPrefs.GetString("ball_" + i) == "selected")
                    PlayerPrefs.SetString("ball_" + i, "bought"); // restul bought ca sa nu existe mai multe selected
                selectionBorder_Balls[i].GetComponent<CanvasRenderer>().cullTransparentMesh = true;
            }
                
    }

    public void CloseValidationPanelBall()
    {
        //selectItem_Sound.Play();
        audioManager.Play("VirtualButton");
        inTimpulUneiPlati = false;
        casetaValidarePlataBall.SetActive(false);
    }
    #endregion

    #region POWERUPS
    public void RequestBuyOrSelectPowerup(int indexNumber) // se pune la fiecare buton in parte
    {

       if (!inTimpulUneiPlati)
       {   
            //Se incearca cumpararea bilei
            if (PlayerPrefs.GetString("powerup_" + powerupType[indexNumber]) == "unbought")
            {
                if (PlayerPrefs.GetInt("Coins") >= pricePowerup[indexNumber])
                {
                    //selectItem_Sound.Play();
                    audioManager.Play("VirtualButton");
                    inTimpulUneiPlati = true;
                    currentIndexNumber = indexNumber;
                    casetaValidarePlataPowerup.SetActive(true);

                }
                //Caseta bani insuf.
                else
                {
                    //insufficientBalance_Sound.Play();
                    audioManager.Play("Error");
                    //inTimpulUneiPlati = true;
                    casetaBaniInsuf.SetActive(true);
                }
            }
        }
    }

    public void PaymentValidationPowerup()
    {
        //buyItem_Sound.Play();
        audioManager.Play("Buy");
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - pricePowerup[currentIndexNumber]);
        coinText.text = PlayerPrefs.GetInt("Coins").ToString();

        PlayerPrefs.SetString("powerup_" + powerupType[currentIndexNumber], "bought");
        lacatPowerups_SHOP[currentIndexNumber].SetActive(false);
        lacatPowerups_PLAY[currentIndexNumber].SetActive(false);
        inTimpulUneiPlati = false;
        casetaValidarePlataPowerup.SetActive(false);
        //selectionBorder_Powerups_SHOP[currentIndexNumber].SetActive(true);
        //selectionBorder_Powerups_PLAY[currentIndexNumber].SetActive(true);
        switch (powerupType[currentIndexNumber])
        {
            case "extra_life":
                Health.extraLife = true;
                Health._Health = 4;
                break;

            case "boost":
                break;

            case "coin_multiplier":
                PlayerPrefs.SetInt("Multiply", 2);
                break;
        }
        //Debug.Log("Vieti:"+Health._Health);
    }

    public void CloseValidationPanelPowerup()
    {
        //selectItem_Sound.Play();
        audioManager.Play("VirtualButton");
        inTimpulUneiPlati = false;
        casetaValidarePlataPowerup.SetActive(false);
    }
    #endregion


    public void CloseInsufficientBalancePanel()
    {
        //selectItem_Sound.Play();
        audioManager.Play("VirtualButton");
        inTimpulUneiPlati = false;
        casetaBaniInsuf.SetActive(false);

    }
}
