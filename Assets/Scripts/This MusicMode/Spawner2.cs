using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner2 : MonoBehaviour
{
   // public GameObject _block;
    public GameObject _player;
    private float _delay;
    private int _pos=1;
    public static bool playerDead;
    public Slider slider;
    public GameObject start, resume;
    //public static float beat_tempo;
    public GameObject _blocks;
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
  
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    void Start()
    {
        PlayerPrefs.SetString("powerup_coin_multiplier", "unbought");
        playerDead = false;
        FirebaseManager.source.time = PlayerPrefs.GetFloat("Music", 0f);
        if (PlayerPrefs.GetFloat("Music", 0f) == 0)
            start.SetActive(true);
        else
            resume.SetActive(true);
        _delay = Health.spawnTime;
        //slider.value = PlayerPrefs.GetFloat("Music", 0) / FirebaseManager.source.clip.length;
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        // Random.Range(spawnTimeBtwTwoConstants[0],spawnTimeBtwTwoConstants[1]);       
        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for(int i=0;i<pool.size;i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    void Update()
    {
        //print(PlayerPrefs.GetFloat("Music"));
        if (playerDead)
            this.gameObject.SetActive(false);
        else if (BallController2.canStart == true )
        {
                if (_delay > 0)
                    _delay -= Time.deltaTime;
                else
                {
                _pos = Random.Range(0, 2);                    
                    if (_pos == 1)
                        SpawnFromPool("Blocks", new Vector3(-2.7f, 11f, 3f), Quaternion.identity); //defY 8.5
                    else
                        SpawnFromPool("Blocks", new Vector3(2.7f, 11f, 3f), Quaternion.identity); //def 3.3
                    _delay = Health.spawnTime;
                }    
        }
        if(!playerDead && BallController2.canStart)
        slider.value = FirebaseManager.source.time / FirebaseManager.source.clip.length;
        else
            slider.value = PlayerPrefs.GetFloat("Music",0) / FirebaseManager.source.clip.length;

    }
    public GameObject SpawnFromPool (string tag,Vector3 position,Quaternion rotation)
    {
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();
       //if(pooledObj !=null)
        
            pooledObj.OnObjectSpawn();
        
        poolDictionary[tag].Enqueue(objectToSpawn);
        return objectToSpawn;
    }
    /*
    IEnumerator DelayCalc()
    {

        k = beat_tempo;
        nr = 1;
        while (k > .5f)
        {
            k = beat_tempo / nr;
            nr += 1;
        }
        yield return null;

    }
    */


}
