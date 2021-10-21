using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartsSystem : MonoBehaviour
{
    public GameObject[] hearts3;
    public GameObject[] hearts4;
    public GameObject rewardHeart;
    // Start is called before the first frame update
    void Start()
    {
        if (Health._Health == 1)
            rewardHeart.SetActive(true);
        else
           if (!Health.extraLife)
            for (int i = 0; i < Health._Health; i++)
                hearts3[i].SetActive(true);
        else
            if (Health.extraLife)
            for (int i = 0; i < Health._Health; i++)
                hearts4[i].SetActive(true);              
    }
    public void Damage()
    {
        if(!Health.extraLife)
            hearts3[Health._Health].SetActive(false);
        else
            hearts4[Health._Health].SetActive(false);
    }      

    public void NoHearts()
    {
        if (!Health.extraLife)
            for (int i = 0; i < 3; i++)
                hearts3[i].SetActive(false);
        else
            for (int i = 0; i < 4; i++)
                hearts4[i].SetActive(false);
    }
}