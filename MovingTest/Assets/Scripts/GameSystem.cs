using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class GameSystem : MonoBehaviour
{
    public Slider reloadingBar;
    float time = 0f;
    public TextMeshProUGUI ammmoCountText;
    public TextMeshProUGUI moneyCurrentText;
    public TextMeshProUGUI enemyRemainText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI AmmoMoneyText;
    public TextMeshProUGUI HealingText;
    public TextMeshProUGUI ShieldingText;
    public bool FirstReload = true;
    PlayerMovement Player;
    public GameObject[] Guns;
    public GameObject MenuUpgrade;
    public GameObject ArrowIndicator;
    public GameObject[] MultMenu;
    public Slider Health;
    public Slider Shield;
    MoneySystem money;
    SpawnManager spawnManager;

    bool IsSet=false;
    public int AmmoMissing = 0;
    private void Start()
    {
        Player = PlayerManager.instance.Player.GetComponent<PlayerMovement>();
        spawnManager = GameObject.FindGameObjectWithTag("Game System").GetComponent<SpawnManager>();
        money = GameObject.FindGameObjectWithTag("Game System").GetComponent<MoneySystem>();
        ShowCurrentMoney();       
    }
    private void Update()
    {
        if (Guns[Player.GunChoose] != null)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                AmmoMissing = 0;
                for (int i = 0; i < Guns.Length; i++)
                {
                    Gun gun = Guns[i].GetComponent<Gun>();
                    AmmoMissing += (gun.maxAmmo - gun.totalAmmo) + (gun.ammoclip - gun.ammo);
                }
                AmmoMoneyText.SetText((AmmoMissing/4) + "$");
            }
            if (Guns[Player.GunChoose].GetComponent<Gun>().isRealoading && FirstReload)
            {
                FirstReload = false;
                time = 0f;
                reloadingBar.gameObject.SetActive(true);
            }
            if (Guns[Player.GunChoose].GetComponent<Gun>().isRealoading) showReloading();
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SetMenu();
            }
        }
    }
    public void SetMenu()
    {
        IsSet = !IsSet;
        if (IsSet)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            for (int i = 1; i < MultMenu.Length; i++) 
            {
                MultMenu[i].SetActive(false);
            }
            MultMenu[0].SetActive(true);
            Time.timeScale = 0f;
            ArrowIndicator.SetActive(false);
        }
        else
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            ArrowIndicator.SetActive(true);
        }
        MenuUpgrade.SetActive(IsSet);
    }
    public void showAmmo(int ammoCount, int ammoMAX)
    {
        ammmoCountText.SetText("Ammo: "+ammoCount + "/"+ ammoMAX);
    }
    public void RefillAmmo()
    {
        for (int i = 0; i < Guns.Length; i++)
        {
            Gun gun = Guns[i].GetComponent<Gun>();
            gun.totalAmmo = gun.maxAmmo;
            gun.ammo = gun.ammoclip;
        }
        Gun GunUsing = Guns[Player.GunChoose].GetComponent<Gun>();
        showAmmo(GunUsing.ammoclip, GunUsing.totalAmmo);
    }
    public void setReloading(float timeTakeReload)
    {
        reloadingBar.maxValue = timeTakeReload;
    }
    public void deleteReloading()
    {
        reloadingBar.gameObject.SetActive(false);
        FirstReload = true;
    }
    void showReloading()
    {        
        time += Time.deltaTime;
        reloadingBar.value = time;
        if (reloadingBar.value == reloadingBar.maxValue)
        {
            if (Guns[Player.GunChoose].GetComponent<Gun>().isSpread&&(Guns[Player.GunChoose].GetComponent<Gun>().ammoclip - Guns[Player.GunChoose].GetComponent<Gun>().ammo)>0&& Guns[Player.GunChoose].GetComponent<Gun>().isRealoading)
            {
                time = 0f;
                reloadingBar.value = time;
            }
            else
            {
                reloadingBar.value = 0;
                reloadingBar.gameObject.SetActive(false);
                FirstReload = true;
            }
        }
    }
    public void ShowCurrentWave(int wave)
    {
        waveText.SetText("Wave: "+(wave+1));
    }
    public void ShowCurrentEnemy()
    {
        enemyRemainText.SetText("Enemy remain: " + spawnManager.EnemyCount);
    }
    public void ShowCurrentMoney()
    {
        moneyCurrentText.SetText("Money: " + money.MoneyCurrent + "$");
    }

    //Slider change value
    public void SetHealthAndShield(float MaxHealth, float MaxShield)
    {
        Health.maxValue = MaxHealth;
        Shield.maxValue = MaxShield;
    }
    public void ChangeHealthValue(float HealthValue)
    {
        Health.value = HealthValue;
    }
    public void ChangeShieldValue(float ShieldValue)
    {
        Shield.value = ShieldValue;
    }
    public void ActiveText(PowerUp power,float time)
    {
        if (power.IsHealing)
        {
            HealingText.gameObject.SetActive(true);
        }
        if (power.IsShield)
        {
            ShieldingText.gameObject.SetActive(true);
        }
        StartCoroutine(CountDown(power, time));
    }
    IEnumerator CountDown(PowerUp type,float duration)
    {
        yield return new WaitForSeconds(duration);
        if (type.IsHealing) HealingText.gameObject.SetActive(false);
        else if (type.IsShield) ShieldingText.gameObject.SetActive(false);
    }
}
