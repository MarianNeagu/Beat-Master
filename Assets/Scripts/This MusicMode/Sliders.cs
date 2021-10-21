using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sliders : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Slider>().value = PlayerPrefs.GetInt(transform.parent.name, 0);
    }


}
