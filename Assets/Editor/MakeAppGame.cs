using UnityEditor;
using UnityEngine;
public class MakeAppGame
{
    [MenuItem("Assets/App is Game")]
    static void AndroidIsGame()
    {
        PlayerSettings.Android.androidIsGame = true;
        Debug.Log("App is now a Game.. I hope");
    }
}
