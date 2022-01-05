using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerX : MonoBehaviour
{
    public GameObject[] objectPrefabs;
    public float spawnInterval = 1.5f;
    private SystemManager SystemManager;
    private PlayerControllerX playerControllerScript;
    private double Timer=0;
    private float randomNumber;

    // Start is called before the first frame update
    void Start()
    {
        SystemManager = GameObject.Find("SystemManager").GetComponent<SystemManager>();
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerControllerX>();
    }
    private void Update()
    {
        if (SystemManager.IsGameStart == true) 
        {
            Timer += Time.deltaTime;
            if (Timer >= spawnInterval)
            {
                SpawnObjects();
                Timer = 0;
            }
        }
    }
    int GenerateRandomNumber()
    {
        int Number =0;
        if (playerControllerScript.IsGoldRush == false)
        {
            randomNumber = Random.Range(0, 1.1f);
            if (randomNumber > 0.425 && randomNumber <= 0.85 ) Number = 1; 
            else if (randomNumber > 0.85 && randomNumber <= 0.9) Number = 2; 
            else if (randomNumber > 0.9 && randomNumber <= 0.95) Number = 3; 
            else if (randomNumber > 0.95 && randomNumber <= 1.1) Number = 4;
        }
        return Number;
    }

    // Spawn obstacles
    void SpawnObjects ()
    {
        // Set random spawn location and random object index
        Vector3 spawnLocation = new Vector3(30, Random.Range(5, 15), 0);
        int index = GenerateRandomNumber();
        // If game is still active, spawn new object
        if (playerControllerScript.gameOver != true)
        {
            Instantiate(objectPrefabs[index], spawnLocation, objectPrefabs[index].transform.rotation);
        }

    }
}
