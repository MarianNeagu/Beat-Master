using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFOV : MonoBehaviour
{
    public Camera camera;
    public float fOV = 91.49284f;
    void Start()
    {
        camera.fieldOfView = fOV;
    }

    
}
