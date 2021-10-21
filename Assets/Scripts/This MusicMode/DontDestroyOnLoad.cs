using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DontDestroyOnLoad : MonoBehaviour
{
    private void Awake()
    {
        int numOfDontDestroyOnLoadObjects = FindObjectsOfType<DontDestroyOnLoad>().Length;
        if (numOfDontDestroyOnLoadObjects > 1)
            Destroy(this.gameObject);
        else
            DontDestroyOnLoad(gameObject);
        
    }
}
