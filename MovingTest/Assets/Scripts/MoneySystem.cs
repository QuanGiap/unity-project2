using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MoneySystem : MonoBehaviour
{
    public int MoneyCurrent = 50;
    public int HealthIncrease = 30;
    public int ShieldIncrease = 30;
    public TextMeshProUGUI MoneyText;
    public float BaseDamageAdd;
    public GameSystem gameSystem;
    public PlayerMovement player;
    int ValueJustUpgraded = 0;
    public int MoneyJustIncrease = 0;
    public TextMeshProUGUI moneyAmmoText;
    // Update is called once per frame
    public void AddMoney(int Amount)
    {
        MoneyCurrent += Amount;
        gameSystem.ShowCurrentMoney();
    }
    public void SpendMoney(int Amount)
    {
        MoneyCurrent -= Amount;
        gameSystem.ShowCurrentMoney();
    }
    public bool IsEnough(int Amount)
    {
        return MoneyCurrent >= Amount;
    }
    public void RefillAmmo()
    {
        if (IsEnough(gameSystem.AmmoMissing / 4))
        {
            SpendMoney(gameSystem.AmmoMissing / 4);
            gameSystem.RefillAmmo();
            moneyAmmoText.SetText("0$");
        }
        else
        {
            MoneyText.gameObject.SetActive(true);
            MoneyText.SetText("Not enough money!!");
            StartCoroutine(waitforText());
        }
    }
    public void UpgradeDamaged(Gun gun)
    {
        int cost = gun.MoneyDamage;
        if (IsEnough(cost)&&gun.IsUnlock)
        {
            SpendMoney(cost);
            cost += Mathf.RoundToInt(cost/5);
            MoneyJustIncrease = cost;
            gun.MoneyDamage = cost;
            gun.damage += gun.MoreDamage;
            ValueJustUpgraded = (int) gun.damage;            
        }
        else if (!gun.IsUnlock)
        {
            MoneyText.gameObject.SetActive(true);
            MoneyText.SetText("Weapons is not Unlock!!");
            StartCoroutine(waitforText());
        }
        else
        {
            MoneyText.gameObject.SetActive(true);
            MoneyText.SetText("Not enough money!!");
            StartCoroutine(waitforText());
        }
    }
    public void IncreaseAccuracy(Gun gun)
    {
        int cost = gun.MoneyAccuracy;
        float deviation = gun.MaxDeviation;
        if (IsEnough(cost) && gun.IsUnlock)
        {
            SpendMoney(cost);
            cost += Mathf.RoundToInt(cost / 3);
            MoneyJustIncrease = cost;
            gun.MoneyAccuracy = cost;
            deviation -= gun.MoreAccuracy;
            ValueJustUpgraded = (int)deviation;
            gun.MaxDeviation = deviation;
            if (deviation <= gun.MaxReduceAccuracy)
            {
                GameObject button = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
                button.SetActive(false);
            }
        }
        else if (!gun.IsUnlock)
        {
            MoneyText.gameObject.SetActive(true);
            MoneyText.SetText("Weapons is not Unlock!!");
            StartCoroutine(waitforText());
        }
        else
        {
            MoneyText.gameObject.SetActive(true);
            MoneyText.SetText("Not enough money!!");
            StartCoroutine(waitforText());
        }
    }
    public void UnlockWeapons(Gun gun)
    {
        if (IsEnough(gun.MoneyUnlock))
        {
            GameObject button = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
            button.SetActive(false);
            SpendMoney(gun.MoneyUnlock);
            gun.IsUnlock = true;
        }
        else
        {
            MoneyText.gameObject.SetActive(true);
            MoneyText.SetText("Not enough money!!");
            StartCoroutine(waitforText());
        }
    }
    public void IncreaseHealthMax()
    {
        int cost = player.MoneyHealth;
        if (IsEnough(cost))
        {
            SpendMoney(cost);
            cost = cost * 2;
            MoneyJustIncrease = cost;
            player.MoneyHealth = cost;
            player.MaxHealth += player.HealthIncrease;
            ValueJustUpgraded =(int) player.MaxHealth;
            player.Health = player.MaxHealth;
            gameSystem.SetHealthAndShield(player.MaxHealth,player.MaxShield);
            gameSystem.ChangeHealthValue(player.Health);
        }
        else
        {
            MoneyText.gameObject.SetActive(true);
            MoneyText.SetText("Not enough money!!");
            StartCoroutine(waitforText());
        }
    }
    public void IncreaseShieldMax()
    {
        int cost = player.MoneyShield;
        if (IsEnough(cost))
        {
            SpendMoney(cost);
            cost = cost * 2;
            MoneyJustIncrease = cost;
            player.MoneyShield = cost;
            player.MaxShield += player.ShieldIncrease;
            ValueJustUpgraded = (int) player.MaxShield;
            player.Shield = player.MaxShield;
            gameSystem.SetHealthAndShield(player.MaxHealth, player.MaxShield);
            gameSystem.ChangeShieldValue(player.Shield);
        }
        else
        {
            MoneyText.gameObject.SetActive(true);
            MoneyText.SetText("Not enough money!!");
            StartCoroutine(waitforText());
        }
    }
    public void IncreaseShieldRegen()
    {
        int cost = player.MoneyShieldRate;
        if (IsEnough(cost))
        {
            SpendMoney(cost);
            cost += cost/2;
            MoneyJustIncrease = cost;
            player.MoneyShieldRate = cost;
            player.ShieldRegenSpeed += player.ShieldRateIncrease;
            ValueJustUpgraded = (int) player.ShieldRegenSpeed;
        }
        else
        {
            MoneyText.gameObject.SetActive(true);
            MoneyText.SetText("Not enough money!!");
            StartCoroutine(waitforText());
        }
    }
    public void RetoreHealthAndShield()
    {
        if (IsEnough(player.MoneyRestore))
        {
            SpendMoney(player.MoneyRestore);
            player.Health = player.MaxHealth;
            player.Shield = player.MaxShield;
            gameSystem.ChangeShieldValue(player.Shield);
            gameSystem.ChangeHealthValue(player.Health);
        }
        else
        {
            MoneyText.gameObject.SetActive(true);
            MoneyText.SetText("Not enough money!!");
            StartCoroutine(waitforText());
        }
    }
    public void ChangeValueText(TextMeshProUGUI text)
    {
        if (ValueJustUpgraded != 0)
        {
            text.SetText(ValueJustUpgraded.ToString());
        }
        ValueJustUpgraded = 0;
    }
    public void ChangeShieldRateText(TextMeshProUGUI text)
    {
        if (ValueJustUpgraded != 0)
        {
            text.SetText(ValueJustUpgraded.ToString() + " Shields/Sec");
        }
        ValueJustUpgraded = 0;
    }
    public void ChangeMoneyText(TextMeshProUGUI text)
    {
        if (MoneyJustIncrease != 0)
        {
            text.SetText(MoneyJustIncrease.ToString() + "$");
        }
        MoneyJustIncrease = 0;
    }
    IEnumerator waitforText()
    {
        yield return new WaitForSecondsRealtime(2f);
        MoneyText.gameObject.SetActive(false);
    }
}
