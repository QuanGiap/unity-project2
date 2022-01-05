using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    [SerializeField] private Transform SpawnPointPosition;
    [SerializeField] private GameObject[] EnemyType;
    [SerializeField] private int Wave = 0;
    [SerializeField] private bool SpawnDone = true;
    [SerializeField] private float TimerWave = 0;
    [SerializeField] private FinishPoint finishPoint;
    [SerializeField] private float spawnRate = 2;
    [SerializeField] private float TimeBetweenWave = 10;
    [SerializeField] private float VaraibleMult = 1;
    private bool FirstTime = true;
    private GUISystem gUISystem;
    private int LengthArray;
    private int[,] ArraySpawnInformation;
    /*
     * There 9 type of enemy
     * Remember to -1 each array to spawn type enemy
     */
    private int i = 0;
    private int ID = 0;
    void Start()
    {
        ArraySpawnInformation  = new int[18,12]
            { { 1, 5, 1, 5, 1, 5, 0, 0, 0, 0, 0, 0},
            { 1, 5, 2, 3, 1, 4, 1, 5, 2, 6, 0, 0},
            { 2, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 1, 10, 2, 5, 3, 2, 1, 4, 3, 2, 0, 0},
            { 3, 6, 1, 15, 2, 6, 3, 6, 0, 0, 0, 0},
            { 3, 18, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 4, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 4, 8, 2, 6, 1, 10, 3, 5, 5, 8, 0, 0},
            { 5, 18, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 1, 5, 3, 3, 2, 3, 4, 10, 6, 4, 0, 0},
            { 4, 5, 3, 3, 6, 4, 5, 2, 6, 2, 0, 0},
            { 6, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 6, 3, 4, 10, 5, 2, 7, 2, 0, 0, 0, 0}, 
            { 7, 1, 4, 6, 7, 1, 1, 5, 7, 2, 0, 0}, 
            { 8, 2, 1, 10, 8, 3, 4, 10, 0, 0, 0, 0}, 
            { 6, 5, 8, 3, 4, 6, 8, 2, 0, 0, 0, 0},
            { 6, 5, 8, 3, 4, 6, 9, 1, 1, 5, 9, 2},
            { 4, 2, 2, 2, 3, 2, 6, 4, 4, 3, 9, 6},
            };
        LengthArray = ArraySpawnInformation.GetLength(1);
        gUISystem = GameSystemManager.Instance.guiSystem;
    }

    // Update is called once per frame
    void Update()
    {
        int EnemyCount = FindObjectsOfType<Enemy>().Length;
        if (EnemyCount == 0 && SpawnDone && FirstTime)
        {
            TimerWave = Time.time + TimeBetweenWave;
            FirstTime = false;
            gUISystem.ActiveNextWaveText(true);
            finishPoint.HealthPlus();
            if ((Wave + 1) % 4 == 0) 
            {
                gUISystem.ShowWarningSign(true);
                StartCoroutine(TurnOffText()); 
            }
            if (Wave == ArraySpawnInformation.GetLength(0))
            {
                gUISystem.IsGameComplete = true;
                gUISystem.IsGameOver = true;
                gUISystem.FreezeGame();
            }
        }
        if (SpawnDone && TimerWave<Time.time && Wave < ArraySpawnInformation.GetLength(0) && EnemyCount == 0)
        {
            gUISystem.ActiveNextWaveText(false);
            SpawnDone = false;
            gUISystem.SetWave(Wave+ 1);
            StartCoroutine(SpawnEnemy());
        }
    }
    IEnumerator SpawnEnemy()
    {
        while (!SpawnDone)
        {
            if (ArraySpawnInformation[Wave, i + 1] == 0) i += 2;
            if (i+1 < LengthArray && ArraySpawnInformation[Wave, i] != 0)
            {
                Spawn();
            }
            else 
            {
                SpawnDone = true;
                Wave++;
                if ((Wave + 1) % 5 == 0) VaraibleMult += 0.8f; 
                i = 0;
                FirstTime = true;
            }
            yield return new WaitForSeconds(1/ spawnRate);
        }
    }
    private int ChooseEnemy()
    {
        ArraySpawnInformation[Wave, i + 1] -= 1;
        return ArraySpawnInformation[Wave, i] - 1;
    }
    private void Spawn()
    {
        if (EnemyType != null)
        {
            GameObject enemy = Instantiate(EnemyType[ChooseEnemy()], SpawnPointPosition.position, SpawnPointPosition.rotation);
            enemy.GetComponent<Enemy>().AdJustingEnemy(VaraibleMult);
            enemy.GetComponent<Enemy>().ID = ID;
            ID++;
            if (ID > 1000) ID = 0;
        }
    }
    IEnumerator TurnOffText()
    {
        yield return new WaitForSeconds(TimeBetweenWave);
        gUISystem.ShowWarningSign(false);
    }
}
