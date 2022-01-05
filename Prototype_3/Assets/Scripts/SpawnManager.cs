using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public float min;
    public float max;
    public float timer;
    public float spawnTime;
    private int randomobject;
    public GameObject[] prefab;
    private PLayerControl playerControl;
    // Start is called before the first frame update
    void Start()
    {
        spawnTime = Random.Range(min, max);
        randomobject = Random.Range(0, prefab.Length);
        playerControl = GameObject.Find("Player").GetComponent<PLayerControl>();
    }
    // Update is called once per frame
    void Update()
    {
        if (playerControl.gameOver == false)
        {
            timer += Time.deltaTime;
            if (timer >= spawnTime)
            {
                Instantiate(prefab[randomobject], new Vector3(33, 0, -1), prefab[randomobject].transform.rotation);
                timer = 0;
                randomobject = Random.Range(0, prefab.Length);
                spawnTime = Random.Range(min, max);
            }
        }
    }
}
