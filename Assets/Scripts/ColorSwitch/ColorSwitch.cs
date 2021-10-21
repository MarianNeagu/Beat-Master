using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ColorSwitch : MonoBehaviour
{
    #region
    public Animator _anim;
    public TextMeshProUGUI _Timer;
    public GameObject _block;
    public float Time_to_Change_Color = 5;
    [SerializeField]
    private float t;
    public Material[] particle;
    private ParticleSystem particles;
    [SerializeField] private float Level_Timer=10f;
    public float Time_To_Spawn_Blocks = 3f;
    public float time_till_spawn= -1;
     public float _size = 7;
     public float forta_frecare = 2;
    public GameObject _player;
    public int rand;
    #endregion

    private void Awake()
    {
        t = Time_to_Change_Color;
    }
    void Start()
    {
        #region Initializare
        particles = GetComponentInChildren<ParticleSystem>();
        _Timer.text = Time_to_Change_Color.ToString();
        _Timer.enabled = false;       
        _size = 7.4f;
        forta_frecare = 3;
        Time_To_Spawn_Blocks = 3;
    #endregion
}

    // Update is called once per frame
    void Update()
    {
        if(Time_To_Spawn_Blocks >= 0.3f && _player != null)
        {
            if (time_till_spawn > 0 && t == -1)
                time_till_spawn -= Time.deltaTime;
            else
            if (time_till_spawn <= 0 && time_till_spawn > -1 && t == -1)
            {
                _block.GetComponent<Rigidbody>().drag = forta_frecare;
                _block.GetComponent<Transform>().localScale = new Vector3(0.25f, _size, 0.25f);
                int _pos = Random.Range(0, 2);
                if (_pos == 1)
                    Instantiate(_block, new Vector3(-2.48f, 9.5f, 10.32f), Quaternion.identity);
                else
                    Instantiate(_block, new Vector3(2.48f, 9.5f, 10.32f), Quaternion.identity);
                time_till_spawn = -1;              
            }
            #region Schimbare interval urmatorul nivel
            if (Level_Timer - _player.GetComponent<PlayerController>()._timer <= 0)
            {
                t = Time_to_Change_Color;
                Level_Timer += 25; //schimbare interval urmatorul nivel
                Time_To_Spawn_Blocks -= 0.2f;
            }
            #endregion
            #region Trecerea de la un nivel la altul
            if (t > 0)
            {
                _Timer.enabled = true;
                t -= Time.deltaTime;
                _Timer.text = t.ToString("0");
                _anim.SetBool("canCount", true);
            }
            else if (t <= 0 && t > -1 )
            {
                _anim.SetBool("canCount", false);
                rand = Random.Range(0, particle.Length);
                particles.GetComponent<ParticleSystemRenderer>().material = particle[rand];
                _Timer.enabled = false;
                t = -1;
                particles.Play();
                if (time_till_spawn == -1)                               
                    time_till_spawn = Time_To_Spawn_Blocks;    
                //Schimbari valori dupa fiecare nivel -->
                forta_frecare -= 0.2f;
                _size -= 0.3f;
            }
            #endregion
        }


    }

}
