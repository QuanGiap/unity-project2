using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GUISystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI MoneyText;
    [SerializeField] private TextMeshProUGUI DescriptionText;
    [SerializeField] private TowerBar BarPrefab;
    [SerializeField] private Transform BarHolder;
    [SerializeField] private LayerMask layer;
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject MenuGame;
    [SerializeField] private TextMeshProUGUI WaveText;
    [SerializeField] private TextMeshProUGUI ChangingWaveText;
    [SerializeField] private TextMeshProUGUI GameMenuText;
    [SerializeField] private bool IsSpeedUp = false;
    [SerializeField] private TextMeshProUGUI WarningSign;
    public bool IsGameOver = false;
    public bool IsGameComplete = false;
    private MoneySystem moneySystem;
    private Dictionary<Transform, TowerBar> BarList = new Dictionary<Transform, TowerBar>();
    #region Delegates
    public static Action<Transform> CreateTowerBar = delegate { };
    public static Action<Transform> CreateEnemyBar = delegate { };
    #endregion

    private void OnEnable()
    {
        CreateTowerBar += SetTowerBarGUI;
        CreateEnemyBar += SetEnemyBarGUI;
    }
    private void OnDisable()
    {
        CreateTowerBar -= SetTowerBarGUI;
        CreateEnemyBar -= SetEnemyBarGUI;
    }

    private void SetTowerBarGUI(Transform Targettransform)
    {
        if (BarList.ContainsKey(Targettransform))
        {
            StartCoroutine(SetBar(Targettransform));
            return;
        }
        TowerScripts towerScripts = Targettransform.gameObject.GetComponent<TowerScripts>();
        TowerBar NewBar = Instantiate(BarPrefab, Targettransform.position,Quaternion.identity,BarHolder);
        BarList.Add(Targettransform,NewBar);
        NewBar.RegisterTower(towerScripts, new Action(() => { BarList.Remove(Targettransform); }));
    }
    private void SetEnemyBarGUI(Transform Targettransform)
    {
        Enemy enemy = Targettransform.gameObject.GetComponent<Enemy>();
        TowerBar NewBar = Instantiate(BarPrefab, transform.position, Quaternion.identity, BarHolder);
        NewBar.RegisterEnemy(enemy);
    }
    private void Start()
    {
        moneySystem = GameSystemManager.Instance.moneySystem;
        SetMoneyText();

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) FreezeGame();
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray,100, layer))
            {
                DescriptionText.SetText("");
            }
            if (!EventSystem.current.IsPointerOverGameObject(-1))
            {
                backButton.onClick.Invoke();
            }
        }
    }
    public void SetWave(int wave)
    {
        WaveText.SetText("Wave: " + wave);
    }
    public void ActiveNextWaveText(bool Active)
    {
        ChangingWaveText.gameObject.SetActive(Active);
    }
    public void SetMoneyText()
    {
        MoneyText.SetText("Money: " + moneySystem.MoneyCurrent+"$");
    }
    public void SetDescriptionText(string text, bool Warning = false)
    {
        if (text != null)
        {
            if (Warning) DescriptionText.fontSize = 24;
            else
                DescriptionText.fontSize = 12;
            DescriptionText.SetText(text);
        }
    }
    IEnumerator SetBar(Transform transform)
    {
        yield return new WaitForSeconds(0.1f);
        BarList[transform].ChangeingMaxBar();
    }
    public void FreezeGame()
    {
        if (Time.timeScale == 0 && !IsGameOver)
        {
            MenuGame.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            MenuGame.SetActive(true);
            Time.timeScale = 0;
            if(IsGameComplete) GameMenuText.SetText("Game Complete");
            else if (IsGameOver) GameMenuText.SetText("Game Over");
        }
    }
    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
    public void SpeedUp()
    {
        if (Time.timeScale != 0)
        {
            if (!IsSpeedUp)
                Time.timeScale = 2;
            else Time.timeScale = 1;
        }
    }
    public void ShowWarningSign(bool active)
    {
        WarningSign.gameObject.SetActive(active);
    }
}
