using UnityEngine;
using EZCameraShake;
using System.Net.Http.Headers;

public class Movement : MonoBehaviour,IPooledObject
{
    //[SerializeField]
   // [Range(0f, 8f)] float lerpTime;
    //[SerializeField] float posY;
   //public float _speed;
    public Material[] mat;
    public Material[] particle;
    public Material coin;
    public int materialNumber;
    //private Rigidbody rb;
    private ParticleSystem particles;
    //public float till_touch;
   // public float _timer;

    //public float beatTempo;
    private HeartsSystem heartsSystem;
    private float calc_time;
    private Animator anim;
    int j, k;
    private bool canKill;
    private Animator animator;
    public Animator touch;

    public AudioSource coinCollectSFX;
    public void OnObjectSpawn()
    {
        canKill = true;
        transform.GetChild(1).gameObject.SetActive(true);      
        int i = Random.Range(0, 4);
        if(i==1)
            transform.GetChild(2).gameObject.SetActive(true);
        materialNumber = mat.Length - 1;
        GetComponent<Renderer>().material = mat[mat.Length - 1];//redevine alb/negru;
        //transform.localScale = new Vector3(0.25f, 0.9f, 0.3f);                        //Backup in movement2.cs
        if (transform.position.x < 0)      
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        transform.GetChild(3).transform.rotation = Quaternion.Euler(Vector3.zero);
        transform.GetChild(4).transform.rotation = Quaternion.Euler(Vector3.zero);
        j = Random.Range(0, 5);
        
        if (j == 1)      
             k = Random.Range(0, 4);   
        else k = -1;
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        particles = GetComponentInChildren<ParticleSystem>();
        heartsSystem = GameObject.FindGameObjectWithTag("Spawner").GetComponent<HeartsSystem>();
        anim = GameObject.FindGameObjectWithTag("Respawn").GetComponent<Animator>();
    }


   
    void Update()
    {
        transform.position -= new Vector3(0f, Health.blocks_speed * Time.deltaTime, 0f);

        if (transform.position.y <= 5.6f && transform.GetChild(2).gameObject.activeSelf == true && Time.timeScale!=1)
            transform.GetChild(2).gameObject.transform.position = Vector3.MoveTowards(transform.GetChild(2).gameObject.transform.position, new Vector3(0, 3.1f, 3), 1);
        if(transform.GetChild(2).gameObject.transform.position == new Vector3(0, 3.1f, 3))
        {
            coinCollectSFX.Play();
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) + 10 * PlayerPrefs.GetInt("Multiply", 1));
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.transform.localPosition = new Vector3(-1.55f, -0.02f, 0.16f);
        }
        if (BallController2.restart_music == 0 && transform.position.y > 3.1f)
            calc_time += Time.deltaTime;
        else
        if (transform.position.y <= 3.1f && BallController2.restart_music == 0)
        {
            BallController2.restart_music = calc_time;
            //print(calc_time + " asta e din movement");
        }

        if (transform.position.y <= 2.28f && !Spawner2.playerDead && canKill)
        {
            if (materialNumber == 5 && Time.timeScale==1)
            {
               if(Health._Health > 0)
                {
                    Health._Health--;
                    heartsSystem.Damage();
                    if (CameraShaker.Instance.ShakeInstances.Count == 0)
                        CameraShaker.Instance.ShakeOnce(2f, 5f, 0f, 0.6f);
                }                  
            }
            canKill = false;
        }
        //print(k);
        if (transform.position.y <= -60f)
            Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            touch.SetTrigger("Touch");
            materialNumber= Random.Range(0, particle.Length);
           
            GetComponent<Renderer>().material = mat[materialNumber];
            if (transform.GetChild(2).gameObject.activeSelf == true)
            {
                if(PlayerPrefs.GetInt("Multiply", 1) == 1)
                animator.SetTrigger("Float");
                else
                    animator.SetTrigger("Float2");
                particles.GetComponent<ParticleSystemRenderer>().material = coin;
                transform.GetChild(2).gameObject.SetActive(false);
                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) + 10 * PlayerPrefs.GetInt("Multiply", 1));
            }
            else
            particles.GetComponent<ParticleSystemRenderer>().material = particle[materialNumber];
            /*
            if (transform.position.x == -2.7f)
                particles.transform.rotation = Quaternion.Euler(0f, 90f, 0f);            
            */
            particles.Play();
            
                if (k == 0)
                    anim.SetTrigger("Great");
                else if (k == 1)
                    anim.SetTrigger("Awesome");
                else if (k == 2)
                    anim.SetTrigger("Incredible");
                else if(k==3)
                    anim.SetTrigger("Wow");
                    
            //rb.drag -= 0.4f;
            transform.GetChild(1).gameObject.SetActive(false);
        }
        
            
        
    }   
    /*
    IEnumerator Speed()
    {
         k = tempo;
        nr = 1.1f;
        while (k <=3.7f)
        {
            k = tempo * nr;
            nr += 0.1f;
        }
        yield return null;
    }
    */
    
}
