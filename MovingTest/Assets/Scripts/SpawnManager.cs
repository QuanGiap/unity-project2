using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SpawnManager : MonoBehaviour
{
    public GameObject SpawnPoints;
    Transform[] spawnpoint;
    public int EnemyCount=0;
    public bool RespawnDone=true;
    public GameObject[] EnemyPrefab;
    public int[] EnemyType;
    public GameObject[] PowerUpType;
    /// <summary>
    /// List enemy will seperate to each class base on EnemyPrefab order,
    /// 0: Pistol
    /// 1: Assault
    /// 2: Shotgun
    /// 3: Smg
    /// 4: Sniper
    /// </summary>
    public List<int> ListEnemy=null;
    public int[,] NumberEnemys;
    public int TotalWave=0;
    public int WaveCurrent=0;
    public float TimeSpawn=3f;
    public float TimeWaveSwitch=10f;
    float timer = 0;
    public float offSet = -2;
    public int MaxWave;
    public int listCountType;
    public GameSystem gameSystem;
    // Start is called before the first frame update
    void Start()
    {
        NumberEnemys = ReadEnemyFilesWave();
        /// <summary>
        /// Need for counting how many spawn point inside the map and register it for each tranform array
        /// </summary>
        spawnpoint = new Transform[SpawnPoints.transform.childCount];
        for (int i = 0; i < SpawnPoints.transform.childCount; i++)
        {
            spawnpoint[i] = SpawnPoints.transform.GetChild(i).transform;
        }
        MaxWave = NumberEnemys.GetLength(0);
        gameSystem.ShowCurrentWave(WaveCurrent);
    }

    // Update is called once per frame
    void Update()
    {
        listCountType = ListEnemy.Count;
        EnemyCount = FindObjectsOfType<Target>().Length;
        gameSystem.ShowCurrentEnemy();
        if (EnemyCount <= 0 && RespawnDone)
        {
            timer += Time.unscaledDeltaTime;
        }
        if (EnemyCount <= 0 && RespawnDone && timer>=TimeWaveSwitch && WaveCurrent != MaxWave)
        {
            CountEnemy(WaveCurrent, NumberEnemys);
            RespawnDone = false;
            gameSystem.ShowCurrentWave(WaveCurrent);
            StartCoroutine(SpawnEnemy());            
        }
    }
    IEnumerator SpawnEnemy()
    {
        timer = 0;
        for (int i = 0; ListEnemy.Count!=0; i++)
        {
            GameObject select = SelectEnemy(WaveCurrent);
            Instantiate(select, spawnpoint[GenerateNumber(0, SpawnPoints.transform.childCount)].position+Vector3.forward* offSet, select.transform.rotation);
            yield return new WaitForSeconds(TimeSpawn);
        }
        RespawnDone=true;
        WaveCurrent++;
        if (WaveCurrent == MaxWave)
        {
            WaveCurrent = 0;
            EnemyUpgraded();
            NumberEnemys = ReadEnemyFilesWave();
        }
    }
    int GenerateNumber(int min,int max)
    { 
        return Random.Range(min,max);
    }
    void CountEnemy(int NumberInHeight,int[,] array)
    {
        EnemyCount = 0;
        ListEnemy.RemoveRange(0,ListEnemy.Count);
        ListEnemy.AddRange(EnemyType);
        int Spaces = 0;
        int i = NumberInHeight;
        for (int j = 0; j < array.GetLength(1); j++)
        {
            EnemyCount += array[i, j];
            if (array[i, j] == 0)
            {
                ListEnemy.RemoveAt(j-Spaces);
                Spaces++;
            }
        }
    }
    GameObject SelectEnemy(int Wave)
    {
            int RandomNumber = GenerateNumber(0, ListEnemy.Count);
            int ChoseNumber = ListEnemy[RandomNumber];
            NumberEnemys[Wave, ChoseNumber] -= 1;
            if (NumberEnemys[Wave, ChoseNumber] == 0)
            {
                ListEnemy.RemoveAt(RandomNumber);
            }
        return EnemyPrefab[ChoseNumber];
    }
    int[,] ReadEnemyFilesWave()
    {
        string readFromFilePath = Application.streamingAssetsPath + "/Resource/" + "WaveInformation" + ".txt";
        System.IO.StreamReader file = new System.IO.StreamReader(readFromFilePath);
        int LineCount = File.ReadAllLines(readFromFilePath).Length;
        LineCount /= 5;  //seperate wave, 5 different enemy each
        int[,] WaveEnemys = new int[LineCount,5];
        TotalWave = LineCount;
        string input = file.ReadLine();
        for (int i = 0; i < LineCount; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                    WaveEnemys[i, j] = int.Parse(input);
                    input = file.ReadLine();
            }
        }
        return WaveEnemys;
    }
    void EnemyUpgraded()
    {
        for (int i = 0; i < EnemyPrefab.Length; i++)
        {
            Target enemy = EnemyPrefab[i].transform.GetChild(1).GetComponent<Target>();
            EnemyGun enemyGun = EnemyPrefab[i].transform.GetChild(1).transform.GetChild(0).GetComponent<EnemyGun>();
            //change enemy stat
            enemy.health *= 2;
            enemy.Money += enemy.Money * 1 / 2;
            //change enemy gun stat
            enemyGun.damage *= 1.75f;
            enemyGun.MaxDeviation -= enemyGun.MaxDeviation * 0.4f;         
        }
        Debug.Log("Enemy upgraded");
    }
    public void DropPowerUp(Transform Postion)
    {
        if (ChanceDrop())
        {
            int number = Random.Range(0, PowerUpType.Length);
            Instantiate(PowerUpType[number], Postion.position, Postion.rotation);
        }
    }
    bool ChanceDrop()
    {
        return ((int) Random.Range(0, 11)) >= 9;
    }
}
