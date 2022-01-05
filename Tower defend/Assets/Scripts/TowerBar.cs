using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TowerBar : MonoBehaviour
{
    private Action DeleteBar;
    private RectTransform rectTransform;
    public Vector3 offset = Vector3.zero;
    [SerializeField] private Slider HealthSlide;
    [SerializeField] private Slider ShieldSlide;
    private Transform TargetTransform;
    private IEnumerator ChangingValue = null;
    private TowerScripts tower;
    private Enemy enemy;
    private void Awake()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
    }
    // Start is called before the first frame update
    public void RegisterTower(TowerScripts towerScripts, Action UnRegister)
    {
        tower = towerScripts;
        HealthSlide.maxValue = tower.Health;
        HealthSlide.value = tower.Health;
        ShieldSlide.maxValue = tower.Shield;
        ShieldSlide.value = tower.Shield;
        DeleteBar = UnRegister;
        rectTransform.position = Camera.main.WorldToScreenPoint(tower.transform.position)+ offset;
        StartCoroutine(ChangeBarTower());
    }
    public void RegisterEnemy(Enemy enemy)
    {
        this.enemy = enemy;
        TargetTransform = enemy.transform;
        HealthSlide.maxValue = enemy.health;
        HealthSlide.value = enemy.health;
        ShieldSlide.maxValue = enemy.Shield;
        ShieldSlide.value = enemy.Shield;
        StartCoroutine(ChangeEnemyTower());
        StartCoroutine(ChangingPosition());
    }
    public void ChangeingMaxBar()
    {
        HealthSlide.maxValue = tower.Health;
        ShieldSlide.maxValue = tower.Shield;
    }
    private IEnumerator ChangeEnemyTower()
    {
        while (enabled)
        {
            if (enemy.health <= 0 || enemy == null)
            {
                Destroy(gameObject);
            }
            HealthSlide.value = enemy.health;
            ShieldSlide.value = enemy.Shield;
            yield return new WaitForSeconds(0.1f);
        }
    }
    private IEnumerator ChangeBarTower()
    {
        while (enabled)
        {
            if (!tower || tower.Health <= 0)
            {
                DeleteBar();
                Destroy(gameObject);
            }
            HealthSlide.value = tower.Health;
            ShieldSlide.value = tower.Shield;
            yield return new WaitForSeconds(0.1f);
        }
    }
    private IEnumerator ChangingPosition()
    {
        while (enabled)
        {
            if(enemy)
                rectTransform.position = Camera.main.WorldToScreenPoint(enemy.transform.position) + offset;
            yield return null;
        }
    }
}
