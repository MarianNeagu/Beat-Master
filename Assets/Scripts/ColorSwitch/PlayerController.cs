using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    #region Variabile (nu,nu fac abuz)
    [SerializeField]
    private float _impulse = 100f;
    public Rigidbody rb;
    private Vector2 mouse;
    private float lastVelocity;
    public float speed;
    private float stayStill;
    public string _scena;
    public float _timer;
    public GameObject colorSwitch;
    public TextMeshProUGUI _score;
   
   
    #endregion
    private void Start()
    {
        _timer = 0;
        _score.text = _timer.ToString("0");
        rb = gameObject.GetComponent<Rigidbody>();
        //Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2 + 10f, 10.32f));
        //transform.position = pos;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        _score.text = _timer.ToString("0");
        #region TouchScreenControll
        if (Input.touchCount > 0 && this.gameObject != null)
        {
            Touch touch = Input.GetTouch(0);
            Camera.main.ScreenToWorldPoint(touch.position);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (touch.position.x > Screen.width / 2)
                    {
                        rb.AddForce(new Vector3(1, 0, 0) * _impulse * Time.deltaTime, ForceMode.Impulse);
                        lastVelocity = 1;
                    }
                    else
                    if (touch.position.x < Screen.width / 2)
                    {
                        rb.AddForce(new Vector3(-1, 0, 0) * _impulse * Time.deltaTime, ForceMode.Impulse);
                        lastVelocity = -1;
                    }
                    break;
            }

        }
        #endregion
        #region KeyboardControll (Debugging only, might delete later)
        if (Input.GetKeyDown("a"))
        {
            rb.AddForce(new Vector3(-1, 0, 0) * _impulse * Time.deltaTime, ForceMode.Impulse);
            lastVelocity = -1;
        }
        if (Input.GetKeyDown("d"))
        {
            rb.AddForce(new Vector3(1, 0, 0) * _impulse * Time.deltaTime, ForceMode.Impulse);
            lastVelocity = 1;
        }

        if (transform.position.x <= -2.69f || transform.position.x >= 2.69f)
        {
            Destroy(this.gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        }
        //lastVelocity = rb.velocity;
        #endregion
        #region Aici am oprit velocitatea sa nu mai faca numerele alea urat la RigidBody :)
        if (rb.velocity.magnitude <= 0.09)
            rb.velocity = new Vector3(0, 0, 0);
        #endregion
    }


    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Block")
        {
            Vector3 direction = Vector3.Reflect(new Vector3(lastVelocity, 0, 0), col.GetContact(0).normal);
            rb.velocity = direction * Mathf.Max(speed, 0);
            colorSwitch.GetComponent<ColorSwitch>().time_till_spawn = colorSwitch.GetComponent<ColorSwitch>().Time_To_Spawn_Blocks;                          
        }

    }
}
