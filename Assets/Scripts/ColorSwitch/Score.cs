using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class Score : MonoBehaviour
{
    public TextMeshProUGUI _score;
    public int _Lives;
    public bool _triggered = false;
    public GameObject colorSwitch;
    private string scene = "ColorSwitch";
    // Start is called before the first frame update
    void Start()
    {
        _Lives = 3;
        _score.text = _Lives.ToString();
        
    }
    // Update is called once per frame
    void Update()
    {
        
        _score.text = "Lives: " + _Lives.ToString();
        if (_Lives <= 0)
            SceneManager.LoadScene(scene);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (_triggered==false && col.tag == "Block" )
            if (col.gameObject.GetComponent<Blocks>().mat_index != colorSwitch.GetComponent<ColorSwitch>().rand)
            {
                _Lives--;
                _triggered = true;
            }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag=="Block")
        _triggered = false;
    }
}
