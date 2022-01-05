using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageSystem : MonoBehaviour
{
    public float DamageOnClick = 20;
    [SerializeField] private float timeShootSpeed;
    [SerializeField] private int DamageIncrease = 7;
    [SerializeField] private LayerMask EnemyLayer;
    [SerializeField] private float ShootSpeed = 5f;
    [SerializeField] private int MoneyDamageCost = 20;
    private MoneySystem moneySystem;
    private float timer = 0;
    private void Start()
    {
        moneySystem = GameSystemManager.Instance.moneySystem;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && timer < Time.time)
        {
            timer = Time.time + 1 / ShootSpeed;
            RaycastHit Enemy;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out Enemy, 100, EnemyLayer))
            {
                Enemy.collider.GetComponent<Enemy>().TakeHit(DamageOnClick);
            }
        }
    }
    public void UpgradeDamageClick(TextMeshProUGUI text)
    {
        if (moneySystem.SpendMoney(MoneyDamageCost))
        {
            MoneyDamageCost += MoneyDamageCost / 4;
            DamageOnClick += DamageIncrease;
        }
        text.SetText("Damage Click Upgrade\n" + MoneyDamageCost + "$");
    }
}
