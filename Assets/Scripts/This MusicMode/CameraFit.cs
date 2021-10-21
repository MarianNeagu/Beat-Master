using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFit : MonoBehaviour
{
    //public Transform limits;
    // Start is called before the first frame update

    private float resW, resH;
    public GameObject cameraObject;

    private float horizontalFOV = 40f;
    private float cameraPos;
    public float raportRezMinim;
    public float raportRezMaxim;
    [Header("Mai apropiat")]
    public float pozCamRezMin;
    [Header("Mai departat")]
    public float pozCamRezMax;
    private float x;
    private void Awake()
    {
        resW = Screen.currentResolution.width;
        resH = Screen.currentResolution.height;
       

        x = Mathf.InverseLerp(raportRezMinim, raportRezMaxim, resH/resW);
        cameraPos = Mathf.Lerp(pozCamRezMin, pozCamRezMax, x);
        cameraObject.gameObject.transform.position = new Vector3(cameraObject.gameObject.transform.position.x, cameraObject.gameObject.transform.position.y, cameraPos);

        #region Obsolete Call
        //camera.fieldOfView = calcVertivalFOV(horizontalFOV, Camera.main.aspect);
        #endregion

    }

    #region Obsolete Function
    /*
    private float calcVertivalFOV(float hFOVInDeg, float aspectRatio)
    {
        float hFOVInRads = hFOVInDeg * Mathf.Deg2Rad;
        float vFOVInRads = 2 * Mathf.Atan(Mathf.Tan(hFOVInRads / 2) / aspectRatio);
        float vFOV = vFOVInRads * Mathf.Rad2Deg;
        return vFOV;
    }*/
    #endregion 

}

