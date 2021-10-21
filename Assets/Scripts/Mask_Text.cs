using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask_Text : MonoBehaviour
{
    // Start is called before the first frame update
    
    private void FixedUpdate()
    {
        GetComponent<MeshRenderer>().material.renderQueue = 3003;
    }

}
