using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AssignedParameters : MonoBehaviour
{
    public static Material ballMat;
    public static bool hasParticleSys;
    public static Color particleSysColor;
    public ParticleSystem particleSystem;
    public int numberOfPowerups;

    void Start()
    {
        gameObject.GetComponent<Renderer>().material = ballMat;
        if (hasParticleSys)
        {
            particleSystem.gameObject.SetActive(true);
            //particleSys.GetComponent<Renderer>().material = particleSysMat;
            particleSystem.startColor = particleSysColor;
        }
        else particleSystem.gameObject.SetActive(false);



    }

}
