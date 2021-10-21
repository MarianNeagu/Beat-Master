using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Blocks : MonoBehaviour
{
    //[SerializeField]
    // [Range(0f, 8f)] float lerpTime;
    //public float _speed;
    public Material[] mat;
    public Material[] particle;
    private Rigidbody rb;
    private ParticleSystem particles;
    //Pentru colorSwitch mode   
    public int mat_index = -1;
   

    private void Awake()
    {      
        particles = GetComponentInChildren<ParticleSystem>();
        particles.gameObject.SetActive(false);
    }
    void Start()
    {
        mat_index = -1;
        rb = GetComponent<Rigidbody>();                    
    }
    void Update()
    {
        if (transform.position.y <= -28)
        {         
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision col)
    {              
            if (col.gameObject.tag == "Player")
            {
            if (mat_index + 1 == mat.Length)
                mat_index = -1;
            mat_index++;          
            particles.gameObject.SetActive(true);
                GetComponent<Renderer>().material = mat[mat_index];
                particles.GetComponent<ParticleSystemRenderer>().material = particle[mat_index];
            
                if (transform.position.x == -2.48f)
                    particles.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                else
                    particles.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
                    particles.Play();           
                            
            }        
    }
}
