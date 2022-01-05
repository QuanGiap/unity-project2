using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneySystem : MonoBehaviour
{
    public int MoneyCurrent = 100;
    private GUISystem guiSystems;
    private void Start()
    {
        guiSystems = GameSystemManager.Instance.guiSystem;
    }
    public void AddMoney(int money)
    {
        MoneyCurrent += money;
        guiSystems.SetMoneyText();
    }
    public bool SpendMoney(int money)
    {
        if (money <= MoneyCurrent)
        {
            MoneyCurrent -= money;
            guiSystems.SetMoneyText();
            return true;
        }
        else
        {
            guiSystems.SetDescriptionText("Not enough money !!!!",true);
            return false;
        }
    }
}
