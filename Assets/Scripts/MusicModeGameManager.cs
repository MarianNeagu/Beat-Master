using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicModeGameManager : MonoBehaviour
{
    [Header("Selectare ultimul level, timeScale pentru fiecare, durata fiecarui level")]
    [Space]
    public int lastLevel;
    public float[] timeScaleArray;
    public float[] levelDurationArray;
    [Space]
    private int currentDifficultyLevel;
    private float levelDuration;
    private float elapsedTime, startTime;
    private bool atLastLevel;
    public static bool inPause;
    public static float currentTimeScale;
    [Header("Audio Source:")]
    public AudioSource music;


    void Start()
    {
        GameManager.currentGameMode = "MusicMode";
        atLastLevel = false;
        currentDifficultyLevel = 1;
        currentTimeScale = 1f;
        Time.timeScale = 1f;
        elapsedTime = 0f;
        inPause = false;

    }

    void Update()
    {
        //print(currentTimeScale);
        currentTimeScale = timeScaleArray[currentDifficultyLevel-1];
        
        if (currentDifficultyLevel == lastLevel)
            atLastLevel = true;

        if (!BallController2.canStart)
            startTime = Time.time;
        else if(!inPause)
        {
            Time.timeScale = timeScaleArray[currentDifficultyLevel - 1];
            levelDuration = levelDurationArray[currentDifficultyLevel - 1];
            music.pitch = Time.timeScale;
            elapsedTime = Time.time - startTime;
            if (elapsedTime > levelDuration && !atLastLevel)
            {
                currentDifficultyLevel++;
                
                //print("NEXT LEVEL!");
            }
        }

        /*print("TIME: "+Time.time);
        print("Time scale: " + Time.timeScale);
        print("Elapsed time: " + elapsedTime);
        print("Level duration: " + levelDuration);*/
    }
}
