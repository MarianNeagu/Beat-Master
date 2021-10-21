using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEditorInternal;

public class MainMenuScript : MonoBehaviour
{
    //VARIABILE
    public GameObject mainMenu, SettingsMenu, CreditsMenu, ShopMenu, MelodySelectMenu;
    public GameObject playCirclesAnim;

    [Header("Animators")]
    
    public Animator buttonsAnimator;
    [Space]
    public Image progress;
    public GameObject loadingScreenCanvas;
    public static float downloadProgress;
    private bool isInMainMenu;
    public GameObject menus;
    public GameObject connectionLost;
    public GameObject retryButton;
    //public GameObject money;
    public TextMeshProUGUI coins;
    public GameObject Powerups_menu;

    public Material[] playAndBackButton_Mat = new Material[2]; // 0 - Play, 1 - Back
    public MeshRenderer playAndBackButton_MeshRend;

    //public AudioManager audioManager;
    
    [Space]

    #region MelodySelect scroll view & Tabs
    [Header("===MelodySelect scroll view & Tabs")]
    public Button[] genresTabs;
    public GameObject[] scrollViewMelodyTabs;

    #endregion

    [Space]

    #region Shop scroll view & Tabs
    [Header("===Shop scroll view & Tabs")]

    public Button[] shopTabs;
    public GameObject[] scrollViewShopTabs;

    #endregion

    [Space]

    #region Variabile Quality
    [Header("===Quality & PostProcessing")]
    public Button[] qualityButtons;
    public Button[] postProcessingButtons;
    public TextMeshProUGUI debugTEXT;
    public Volume volume;
    #endregion

    [Space]

    #region Variabile Slider
    [Header("===Audio")]
    private Touch touch;
    public float volumeSlider_speed;
    public RectTransform uiButtonSlider;
    public static bool isTouchingSlider;
    private bool isSliderUp;
    private bool isSliderDown;
    private float x;
    #endregion 

    [Space]

    #region Audio
    [Header("===Audio")]
    public AudioManager audioManager;
    //public AudioSource bgMusic;
    //public AudioSource[] fxSounds; //0 - mechButton;  1 - platan Play;  2 - virtualButton;  3 - openCase;  4 - 
    #endregion

    [Space]

    #region Score
    [Header("===Score")]
    public TextMeshProUGUI highScoreText;
    #endregion

    [Space]

    #region Search Bar
    [Header("===Search Bar")]
    public TMP_InputField searchBar;
    public TextMeshProUGUI[] melody_TextName;
    public GameObject[] melody_gameObject;
    #endregion



    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }
    //public ShopSystem shopSystem;
    private void Start()
    {
        playCirclesAnim.SetActive(true);
        
        
        audioManager.Play("PickupOpening");
        //Debug.Log(PlayerPrefs.GetInt("Current_Res_W"));
        Invoke("ActivateMenus", 2.6f);
        downloadProgress = 0;
        loadingScreenCanvas.SetActive(false);
        isInMainMenu = true;
        #region QUALITY

        //volume = FindObjectOfType<Volume>();
        /*
        if (!PlayerPrefs.HasKey("GV_weight"))
            volume.weight = 1f;
        else volume.weight = PlayerPrefs.GetFloat("GV_weight");
        */

        if (!PlayerPrefs.HasKey("Quality_Level")) // Primul joc ever
        {
            PlayerPrefs.SetInt("Phone_Res_W", Screen.currentResolution.width);
            PlayerPrefs.SetInt("Phone_Res_H", Screen.currentResolution.height);
            PlayerPrefs.SetInt("Current_Res_W", Screen.currentResolution.width);
            PlayerPrefs.SetInt("Current_Res_H", Screen.currentResolution.height);
            PlayerPrefs.SetString("Quality_Level", "high");
            qualityButtons[0].interactable = true;
            qualityButtons[1].interactable = true;
            qualityButtons[2].interactable = false;
        }
        //Dupa primul joc
        else
        {
            Screen.SetResolution(PlayerPrefs.GetInt("Current_Res_W"), PlayerPrefs.GetInt("Current_Res_H"), true);
            if (PlayerPrefs.GetString("Quality_Level") == "high")
            {
                qualityButtons[0].interactable = true;
                qualityButtons[1].interactable = true;
                qualityButtons[2].interactable = false;
            }
            else if (PlayerPrefs.GetString("Quality_Level") == "medium")
            {
                qualityButtons[0].interactable = true;
                qualityButtons[1].interactable = false;
                qualityButtons[2].interactable = true;
            }
            else
            {
                qualityButtons[0].interactable = false;
                qualityButtons[1].interactable = true;
                qualityButtons[2].interactable = true;
            }
        }
        /*
        if (volume.weight == 0)
        {
            postProcessingButtons[0].interactable = false;
            postProcessingButtons[1].interactable = true;
            postProcessingButtons[2].interactable = true;
        }
        else if (volume.weight == 0.5f)
        {
            postProcessingButtons[0].interactable = true;
            postProcessingButtons[1].interactable = false;
            postProcessingButtons[2].interactable = true;
        }
        else
        {
            postProcessingButtons[0].interactable = true;
            postProcessingButtons[1].interactable = true;
            postProcessingButtons[2].interactable = false;
        }
        */

        
        #endregion

        Time.timeScale = 1f;
        
        isTouchingSlider = false;
        
        isInMainMenu = true;
        mainMenu.SetActive(true);
        menus.SetActive(false);
        SettingsMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        ShopMenu.SetActive(false);
        MelodySelectMenu.SetActive(false);
        Powerups_menu.SetActive(false);
        playAndBackButton_MeshRend.material = playAndBackButton_Mat[0];

        #region MelodySelect scrollView 
        scrollViewMelodyTabs[0].SetActive(true);
        genresTabs[0].interactable = false;
        for (int i = 1; i < genresTabs.Length; i++)
        { 
            genresTabs[i].interactable = true;
            scrollViewMelodyTabs[i].SetActive(false);
        }
        #endregion

        #region Shop scrollView 
        scrollViewShopTabs[0].SetActive(true);
        shopTabs[0].interactable = false;
        for(int i = 1; i < shopTabs.Length; i++)
        {
            shopTabs[i].interactable = true;
            scrollViewShopTabs[i].SetActive(false);
        }
        #endregion

        if(!PlayerPrefs.HasKey("audio_volume"))
        {
            isSliderUp = true;
            isSliderDown = false;
        }
        else
        {
            uiButtonSlider.localPosition = new Vector3(PlayerPrefs.GetFloat("audio_slider_posX"), PlayerPrefs.GetFloat("audio_slider_posY"), PlayerPrefs.GetFloat("audio_slider_posZ"));
            for (int i = 0; i < audioManager.sounds.Length; i++)
            {
                audioManager.sounds[i].volume = PlayerPrefs.GetFloat("audio_volume");
                audioManager.sounds[i].source.volume = PlayerPrefs.GetFloat("audio_volume");
            }
        }

        /*if (!PlayerPrefs.HasKey("HighScore"))
            highScoreText.text = "0";
        else highScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();
        */
        coins.text = PlayerPrefs.GetInt("Coins", 0).ToString();

    }
    void Update()
    {            
        if (progress.IsActive() )
            progress.fillAmount = downloadProgress;

        #region Miscare Slider Volum

        

        //Verificare limita slider
        if (uiButtonSlider.anchoredPosition.y >= -220f)
            isSliderUp = true;
        else isSliderUp = false;

        if (uiButtonSlider.anchoredPosition.y <= -340f)
            isSliderDown = true;
        else isSliderDown = false;

        //Fixare pozitie daca trece de limita santului
        if (uiButtonSlider.anchoredPosition.y > -220)
            uiButtonSlider.localPosition = new Vector3(260f, -580f, -80f);
        
        if (uiButtonSlider.anchoredPosition.y < -340)
            uiButtonSlider.localPosition = new Vector3(260f, -700.1466f, -167.2912f);

        if (isTouchingSlider && Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            x = Mathf.InverseLerp(-340f, -220f, uiButtonSlider.anchoredPosition.y);
            //bgMusic.volume = Mathf.Lerp(0f, 1f, x);
            PlayerPrefs.SetFloat("audio_volume", Mathf.Lerp(0f, 1f, x));
            PlayerPrefs.SetFloat("audio_slider_posX", uiButtonSlider.localPosition.x);
            PlayerPrefs.SetFloat("audio_slider_posY", uiButtonSlider.localPosition.y);
            PlayerPrefs.SetFloat("audio_slider_posZ", uiButtonSlider.localPosition.z);
            for (int i = 0; i < audioManager.sounds.Length; i++)
            {
                audioManager.sounds[i].volume = Mathf.Lerp(0f, 1f, x);
                audioManager.sounds[i].source.volume = Mathf.Lerp(0f, 1f, x);
            }

            

            if (isSliderUp && touch.deltaPosition.y < 0)
                uiButtonSlider.position += uiButtonSlider.transform.up * touch.deltaPosition.y * volumeSlider_speed * Time.deltaTime;
            else if(isSliderDown && touch.deltaPosition.y > 0)
                uiButtonSlider.position += uiButtonSlider.transform.up * touch.deltaPosition.y * volumeSlider_speed * Time.deltaTime;
            else if (!isSliderDown && !isSliderUp)
                uiButtonSlider.position += uiButtonSlider.transform.up * touch.deltaPosition.y * volumeSlider_speed * Time.deltaTime;
        }

        #endregion

        
            

    }

    #region Functii Slider
    public void onVolumeSliderPress()
    {
        //fxSounds[2].Play();
        audioManager.Play("VirtualButton");
        isTouchingSlider = true;
    }

    public void onVolumeSliderRelease()
    {
        isTouchingSlider = false;
    }
    #endregion

    #region SearchBox
    public void OnSelectSearchBar()
    {
        //fxSounds[2].Play();
        audioManager.Play("VirtualButton");
    }

    public void OnTyping()
    {
        Powerups_menu.SetActive(false);
        audioManager.Play("VirtualButton");
        //Debug.Log("Typing...");
        for (int i = 0; i < melody_TextName.Length; i++)
        {
            if (searchBar.text != null)
            {
                if (!melody_TextName[i].text.ToLower().Contains(searchBar.text.ToLower()))
                    melody_gameObject[i].SetActive(false);
                else melody_gameObject[i].SetActive(true);
            }
                
            else melody_gameObject[i].SetActive(true);
        }
    }

    #endregion

    public void ActivateMenus()
    {
        menus.SetActive(true);
        audioManager.Play("MenuMusic");
    }

    #region MainMenu Mechanical Buttons
    public void Play()
    {
        //fxSounds[1].Play();
        playCirclesAnim.SetActive(false);
        audioManager.Play("PlatanButton");
        Powerups_menu.SetActive(false);
        buttonsAnimator.Play("Pickup_PlayButton");
        SettingsMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        ShopMenu.SetActive(false);
        mainMenu.SetActive(false);
        MelodySelectMenu.SetActive(true);
        playAndBackButton_MeshRend.material = playAndBackButton_Mat[2];
    }
   
    public void Settings()
    {
        mainMenu.SetActive(false);
        //fxSounds[0].Play();
        audioManager.Play("MecButton");
        Powerups_menu.SetActive(false);
        buttonsAnimator.Play("Pickup_Button");
        //GameModeSelectMenu.SetActive(false);
        SettingsMenu.SetActive(true);
        CreditsMenu.SetActive(false);
        ShopMenu.SetActive(false);
        MelodySelectMenu.SetActive(false);
        isInMainMenu = false;
        playAndBackButton_MeshRend.material = playAndBackButton_Mat[1];
    }

    public void Shop()
    {
        mainMenu.SetActive(false);
        //fxSounds[0].Play();
        audioManager.Play("MecButton");
        Powerups_menu.SetActive(false);
        buttonsAnimator.Play("Pickup_Button1");
        //GameModeSelectMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        ShopMenu.SetActive(true);
        MelodySelectMenu.SetActive(false);
        isInMainMenu = false;
        playAndBackButton_MeshRend.material = playAndBackButton_Mat[1];
    }

    public void Credits()
    {
        mainMenu.SetActive(false);
        //fxSounds[0].Play();
        audioManager.Play("MecButton");
        buttonsAnimator.Play("Pickup_Button2");
        Powerups_menu.SetActive(false);
        //GameModeSelectMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        CreditsMenu.SetActive(true);
        ShopMenu.SetActive(false);
        MelodySelectMenu.SetActive(false);
        isInMainMenu = false;
        playAndBackButton_MeshRend.material = playAndBackButton_Mat[1];
    }

    public void Quit()
    {
        //fxSounds[0].Play();
        audioManager.Play("MecButton");
        buttonsAnimator.Play("Pickup_Button3");
        Application.Quit();
    }
    #endregion

    #region GameModeSelectMenu Buttons

    public void ArcadeMode()
    {
        SceneManager.LoadScene("ColorSwitch");

    }
    #endregion

    #region SettingsMenu Buttons

    public void LowQuality()
    {
        //fxSounds[2].Play();
        audioManager.Play("VirtualButton");
        Screen.SetResolution(PlayerPrefs.GetInt("Phone_Res_W") / 2, PlayerPrefs.GetInt("Phone_Res_H") / 2, true);
        PlayerPrefs.SetInt("Current_Res_W", PlayerPrefs.GetInt("Phone_Res_W") / 2);
        PlayerPrefs.SetInt("Current_Res_H", PlayerPrefs.GetInt("Phone_Res_H") / 2);
        PlayerPrefs.SetString("Quality_Level", "low");
        qualityButtons[0].interactable = false;
        qualityButtons[1].interactable = true;
        qualityButtons[2].interactable = true;
    }

    public void MediumQuality()
    {
        //fxSounds[2].Play();
        audioManager.Play("VirtualButton");
        Screen.SetResolution((int)(PlayerPrefs.GetInt("Phone_Res_W") / 1.5f), (int)(PlayerPrefs.GetInt("Phone_Res_H") / 1.5f), true);
        PlayerPrefs.SetInt("Current_Res_W", (int)(PlayerPrefs.GetInt("Phone_Res_W") / 1.5f));
        PlayerPrefs.SetInt("Current_Res_H", (int)(PlayerPrefs.GetInt("Phone_Res_H")/1.5f));
        PlayerPrefs.SetString("Quality_Level", "medium");
        qualityButtons[0].interactable = true;
        qualityButtons[1].interactable = false;
        qualityButtons[2].interactable = true;
    }

    public void HighQuality()
    {
        //fxSounds[2].Play();
        audioManager.Play("VirtualButton");
        Screen.SetResolution(PlayerPrefs.GetInt("Phone_Res_W"), PlayerPrefs.GetInt("Phone_Res_H"), true);
        PlayerPrefs.SetInt("Current_Res_W", PlayerPrefs.GetInt("Phone_Res_W"));
        PlayerPrefs.SetInt("Current_Res_H", PlayerPrefs.GetInt("Phone_Res_H"));
        PlayerPrefs.SetString("Quality_Level", "high");
        qualityButtons[0].interactable = true;
        qualityButtons[1].interactable = true;
        qualityButtons[2].interactable = false;
    }

    public void LowPP()
    {
        //fxSounds[2].Play();
        audioManager.Play("VirtualButton");
        volume.weight = 0f;
        
        PlayerPrefs.SetFloat("GV_weight", volume.weight);
        postProcessingButtons[0].interactable = false;
        postProcessingButtons[1].interactable = true;
        postProcessingButtons[2].interactable = true;
    }

    public void MediumPP()
    {
        //fxSounds[2].Play();
        audioManager.Play("VirtualButton");
        volume.weight = 0.5f;
        PlayerPrefs.SetFloat("GV_weight", volume.weight);
        postProcessingButtons[0].interactable = true;
        postProcessingButtons[1].interactable = false;
        postProcessingButtons[2].interactable = true;
    }

    public void HighPP()
    {
        //fxSounds[2].Play();
        audioManager.Play("VirtualButton");
        volume.weight = 1f;
        PlayerPrefs.SetFloat("GV_weight", volume.weight);
        postProcessingButtons[0].interactable = true;
        postProcessingButtons[1].interactable = true;
        postProcessingButtons[2].interactable = false;
    }

    #endregion

    public void SelectSong()
    {
        Powerups_menu.SetActive(true);
        MelodySelectMenu.SetActive(false);  
        SettingsMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        ShopMenu.SetActive(false);
        mainMenu.SetActive(false);

        //fxSounds[2].Play();
        audioManager.Play("VirtualButton");
        FirebaseManager.buttonName = EventSystem.current.currentSelectedGameObject.name;
        

    }

    public void StartGame()
    {
        //fxSounds[2].Play();
        audioManager.Play("VirtualButton");
        loadingScreenCanvas.SetActive(true);
        audioManager.Stop("MenuMusic");
        FirebaseManager firebaseManager = GameObject.FindGameObjectWithTag("AudioSource").GetComponent<FirebaseManager>();
        if (menus.active == true && (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork))
            menus.SetActive(false);
        firebaseManager.DownloadMelody();     
    }

    public void CloseButton()
    {
        //fxSounds[2].Play();
        audioManager.Play("VirtualButton");
        Powerups_menu.SetActive(false);
        MelodySelectMenu.SetActive(true);
    }

    #region Genres Tabs
    public void GenreTab(int i) 
    {
        //fxSounds[2].Play();
        audioManager.Play("VirtualButton");
        genresTabs[i].interactable = false;
        scrollViewMelodyTabs[i].SetActive(true);
        Powerups_menu.SetActive(false);
        for (int j = 0; j < genresTabs.Length; j++)
            if (j != i)
            {
                genresTabs[j].interactable = true;
                scrollViewMelodyTabs[j].SetActive(false);
            }   
    }

    

    #endregion

    #region Shop Tabs

    public void ShopTab0()
    {
        //fxSounds[2].Play();
        audioManager.Play("VirtualButton");
        shopTabs[0].interactable = false;
        scrollViewShopTabs[0].SetActive(true);
        for (int i = 0; i < shopTabs.Length; i++)
            if (i != 0)
            {
                shopTabs[i].interactable = true;
                scrollViewShopTabs[i].SetActive(false);
            }
    }

    public void ShopTab1()
    {
        //fxSounds[2].Play();
        audioManager.Play("VirtualButton");
        shopTabs[1].interactable = false;
        scrollViewShopTabs[1].SetActive(true);
        for (int i = 0; i < shopTabs.Length; i++)
            if (i != 1)
            {
                shopTabs[i].interactable = true;
                scrollViewShopTabs[i].SetActive(false);
            }
    }

    #endregion
}
