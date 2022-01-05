using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class BuildSystem : MonoBehaviour
{
    [SerializeField] private LayerMask towerLayer;
    private GameObject gameObjectCurrent;
    private GameObject ObjectUpgrade;
    private GUISystem gUISystem;
    private MoneySystem moneySystem;
    public Button BuyingButton;
    [SerializeField] private int repairCost = 20;
    [SerializeField] private Button UpgradeButton;
    [SerializeField] private Button HealButton;
    [SerializeField] private Button BackButton;
    [SerializeField] private Button FriendlyFireOffButton;
    [SerializeField] private Button[] ButtonUpgrades;
    [SerializeField] private Button RepairButton;
    private static GameObject ObjectDrawed;
    private bool OnlyChangeMoney;
    public Button SellingButton;
    public bool IsBuying = false;
    [SerializeField] private GameObject PositionPlacing;
    [SerializeField] private Vector3 mousePos;
    public bool CanBuild = true;
    private bool IsUpgrade = false;
    private void Start()
    {
        gUISystem = GameSystemManager.Instance.guiSystem;
        moneySystem = GameSystemManager.Instance.moneySystem;
        SellingButton.interactable = false;
    }
    private void Update()
    {
        if (IsUpgrade && ObjectUpgrade == null) BackButton.onClick.Invoke();
        if (IsBuying && PositionPlacing != null)
            PositionPlacing.transform.position = setPosition();
        if (Input.GetMouseButtonDown(0) && IsBuying && !EventSystem.current.IsPointerOverGameObject(-1))
        {
             StartCoroutine(SetBuilding());
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit Tower;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out Tower,100,towerLayer))
            {
                gameObjectCurrent = Tower.collider.gameObject;
                SellingButton.interactable = true;
                UpgradeButton.interactable = true;
                RepairButton.interactable = true;
                BuyingButton.interactable = false;
            }
            else if (!EventSystem.current.IsPointerOverGameObject(-1))
            {
                SellingButton.interactable = false;
                RepairButton.interactable = false;
                UpgradeButton.interactable = false;
                BuyingButton.interactable = false;
            }
        }
    }
    public static void DrawCircle(GameObject container, float radius=0, float lineWidth=0)
    {
        if (ObjectDrawed != null) ObjectDrawed.GetComponent<LineRenderer>().positionCount = 0;
        int segments = 360;
        LineRenderer line = container.GetComponent<LineRenderer>();
        if(!line)
        {
            line = container.AddComponent<LineRenderer>();
        }
        line.useWorldSpace = false;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.positionCount = segments + 1;

        int pointCount = segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
        var points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, 1.2f, Mathf.Cos(rad) * radius);
        }
        line.SetPositions(points);
        ObjectDrawed = container;
    }
    public void GetBuildingInform(GameObject gameObject)
    {
        gameObjectCurrent = gameObject;
        string str = gameObjectCurrent.GetComponent<TowerScripts>().SetDescription();
        gUISystem.SetDescriptionText(str);
    }
    public void Buying(TextMeshProUGUI text)
    {
        if (gameObjectCurrent != null)
        {
            IsBuying = !IsBuying;
            if (IsBuying)
            {
                PositionPlacing.SetActive(true);
                BuyingButton.GetComponent<Image>().color = Color.red;
                text.SetText("Cancel");
                DrawCircle(PositionPlacing, gameObjectCurrent.GetComponent<TowerScripts>().RadiusCheck, 0.1f);
                SellingButton.interactable = false;
            }
            else 
            {
                ClearBuildingCircle();
            }
        }
    }
    public void DemolishBuild()
    {
        moneySystem.AddMoney(gameObjectCurrent.GetComponent<TowerScripts>().MoneyCost / 2);
        Destroy(gameObjectCurrent);
        SellingButton.interactable = false;
        UpgradeButton.interactable = false;
        RepairButton.interactable = false;
    }
    private Vector3 setPosition()
    {
        Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(PositionPlacing.transform.position).z);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(position);
        return new Vector3(worldPos.x, 0, worldPos.z);
    }
    private void BuildingTheTower()
    {
        bool IsEnough = moneySystem.SpendMoney(gameObjectCurrent.GetComponent<TowerScripts>().MoneyCost);
        if (CanBuild && IsEnough)
        {
            GameObject ObjectPosition = Instantiate(gameObjectCurrent, PositionPlacing.transform.position, PositionPlacing.transform.rotation);
            gameObjectCurrent = null;
            ClearBuildingCircle();
            IsBuying = false;
            GUISystem.CreateTowerBar(ObjectPosition.transform);
        }
        else if (!IsEnough) BuyingButton.onClick.Invoke();
    }
    private void ClearBuildingCircle()
    {
        PositionPlacing.SetActive(false);
        BuyingButton.GetComponent<Image>().color = Color.white;
        BuyingButton.GetComponentInChildren<TextMeshProUGUI>().SetText("Buy");
        DrawCircle(PositionPlacing);
    }
    IEnumerator SetBuilding()
    {
        yield return new WaitForSeconds(0.1f);
        BuildingTheTower();
    }
    public void SetUpObjectUpgrade()
    {
        if (gameObjectCurrent)
        {
            ObjectUpgrade = gameObjectCurrent;
            HealingScript healingScript = ObjectUpgrade.GetComponent<HealingScript>();
            TowerScripts tower = ObjectUpgrade.GetComponent<TowerScripts>();
            if (healingScript) HealButton.interactable = true;
            else HealButton.interactable = false;
            if (ObjectUpgrade.GetComponent<TowerScripts>().IsRocketTower)
                FriendlyFireOffButton.interactable = true;
            else FriendlyFireOffButton.interactable = false;
            OnlyChangeMoney = true;
            foreach (Button button in ButtonUpgrades)
            {
                button.onClick.Invoke();
            }
            OnlyChangeMoney = false;
            IsUpgrade = true;
        }
        else BackButton.onClick.Invoke();
    }
    //Shop Upgrade
    public void SetUpgradeFalse()
    {
        IsUpgrade = false;
    }
    public void IncreaseFireRate(TextMeshProUGUI text)
    {
        TowerScripts towerScripts = ObjectUpgrade.GetComponent<TowerScripts>();
        if (!OnlyChangeMoney && moneySystem.SpendMoney(towerScripts.FireRateCost))
        {
            towerScripts.fireRate += towerScripts.FireRateIncrease;
            towerScripts.FireRateCost += towerScripts.FireRateCost*2/3;
            gUISystem.SetDescriptionText(towerScripts.SetDescription());
        }
        text.SetText(towerScripts.FireRateCost + "$");
    }
    public void IncreaseRadius(TextMeshProUGUI text)
    {
        TowerScripts towerScripts = ObjectUpgrade.GetComponent<TowerScripts>();
        HealingScript healing = ObjectUpgrade.GetComponent<HealingScript>();
        if (!OnlyChangeMoney && moneySystem.SpendMoney(towerScripts.RadiusCost))
        {
            towerScripts.RadiusCheck += towerScripts.RadiusIncrease;
            if (healing) healing.radius = towerScripts.RadiusCheck;
            towerScripts.RadiusCost += towerScripts.RadiusCost * 2 / 3;
        }
        text.SetText(towerScripts.RadiusCost + "$");
    }
    public void IncreaseDamage(TextMeshProUGUI text)
    {
        TowerScripts towerScripts = ObjectUpgrade.GetComponent<TowerScripts>();
        if (!OnlyChangeMoney && moneySystem.SpendMoney(towerScripts.DamageCost))
        {
            towerScripts.Damage += towerScripts.DamageIncrease;
            towerScripts.DamageCost += towerScripts.DamageCost * 2 / 3;
            gUISystem.SetDescriptionText(towerScripts.SetDescription());
        }
        text.SetText(towerScripts.DamageCost + "$");
    }
    public void IncreaseHealth(TextMeshProUGUI text)
    {
        TowerScripts towerScripts = ObjectUpgrade.GetComponent<TowerScripts>();
        if (!OnlyChangeMoney && moneySystem.SpendMoney(towerScripts.HealthCost))
        {
            towerScripts.MaxHealth += towerScripts.HealthIncrease;
            GUISystem.CreateTowerBar(towerScripts.transform);//reset bar
            towerScripts.Health = towerScripts.MaxHealth;
            towerScripts.HealthCost += towerScripts.HealthCost * 2 / 3;
            gUISystem.SetDescriptionText(towerScripts.SetDescription());
        }
        text.SetText(towerScripts.HealthCost + "$");

    }
    public void IncreaseShield(TextMeshProUGUI text)
    {
        TowerScripts towerScripts = ObjectUpgrade.GetComponent<TowerScripts>();
        if (!OnlyChangeMoney && moneySystem.SpendMoney(towerScripts.ShieldCost))
        {
            towerScripts.MaxShield += towerScripts.ShieldIncrease;
            GUISystem.CreateTowerBar(ObjectUpgrade.transform);//reset bar
            towerScripts.Shield = towerScripts.MaxShield;
            towerScripts.ShieldCost += towerScripts.ShieldCost* 2 / 3;
            gUISystem.SetDescriptionText(towerScripts.SetDescription());
        }
        text.SetText(towerScripts.ShieldCost + "$");
    }
    public void IncreaseShieldRate(TextMeshProUGUI text)
    {
        TowerScripts towerScripts = ObjectUpgrade.GetComponent<TowerScripts>();
        if (!OnlyChangeMoney && moneySystem.SpendMoney(towerScripts.ShieldRegenCost))
        {
            towerScripts.RegenShieldSpeed += towerScripts.ShieldRegenSpeedIncrease;
            towerScripts.ShieldRegenCost += towerScripts.ShieldRegenCost * 2 / 3;
            gUISystem.SetDescriptionText(towerScripts.SetDescription());
        }
        text.SetText(towerScripts.ShieldRegenCost + "$");
    }
    public void HealRateIncrease(TextMeshProUGUI text)
    {
        HealingScript towerHeal = ObjectUpgrade.GetComponent<HealingScript>();
        if (towerHeal)
        {
            TowerScripts towerScripts = ObjectUpgrade.GetComponent<TowerScripts>();
            if (!OnlyChangeMoney && moneySystem.SpendMoney(towerScripts.HealingRateCost))
            {
                towerHeal.healSpeed += towerScripts.HealingRateIncrease;
                towerScripts.HealingRateCost += towerScripts.HealingRateCost * 2 / 3;
            }
            text.SetText(towerScripts.HealingRateCost + "$");
        }
        else text.SetText("");
    }
    public void RepairStructure()
    {
        if (moneySystem.SpendMoney(repairCost))
        {
            TowerScripts tower = gameObjectCurrent.GetComponent<TowerScripts>();
            if (tower)
            {
                tower.Health = tower.MaxHealth;
                tower.Shield = tower.MaxShield;
            }
        }
    }
    public void DisableFriendlyFire()
    {
        TowerScripts towerScripts = ObjectUpgrade.GetComponent<TowerScripts>();
        if (!OnlyChangeMoney && moneySystem.SpendMoney(60))
        {
            towerScripts.IsRocketTower = false;
            FriendlyFireOffButton.interactable = false;
        }
    }
}
