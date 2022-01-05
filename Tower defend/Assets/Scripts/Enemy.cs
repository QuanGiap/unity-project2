using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour , IDamageable
{
    [SerializeField] public float health = 20;
    [SerializeField] public float Shield = 0;
    [SerializeField] private float maxShield = 0;
    [SerializeField] private float TimeToRegen = 3;
    [SerializeField] private float timer = 0;
    [SerializeField] private float damage = 10;
    private MoneySystem moneySystem;
    private IEnumerator SLowDownIE = null;
    [SerializeField] private float speed = 2;
    [SerializeField] private bool CanSlow = true;
    [SerializeField] private int money = 10;
    [SerializeField] private bool CanAttack = false;
    [SerializeField] private float ShootSpeed = 1;
    [SerializeField] private float RadiusCheck = 4f;
    [SerializeField] private LayerMask Towerlayer;
    [SerializeField] private LayerMask Defendlayer;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float RegenShieldSpeed = 5;
    public int ID = 0;
    private void Awake()
    {
        moneySystem = GameSystemManager.Instance.moneySystem;
        StartCoroutine(WaitForCreatBar());
        if (CanAttack)
        {
            StartCoroutine(CheckingEnemy());
        }
    }
    void FixedUpdate()
    {
        transform.Translate(Vector3.forward*speed*Time.deltaTime);
        if (timer < Time.time) ShieldRegen();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PathPointer"))
        {
            transform.rotation = other.transform.rotation;
        }
    }
    public void TakeHit(float damage)
    {
        timer = Time.time + TimeToRegen;
        if (Shield > 0)
        {
            Shield -= damage;
            if (Shield < 0)
            {
                damage = -1 * Shield;
                Shield = 0;
                health -= damage;
            }
        }
        else
        {
            health -= damage;
        }
        if (health <= 0) DestroyObject();
    }
    public void SlowDownEnemy(float timer)
    {
        if (CanSlow)
        {
            if (SLowDownIE != null)
            {
                speed *= 2;
                StopCoroutine(SLowDownIE);
            }
            SLowDownIE = SlowDown(timer);
            StartCoroutine(SLowDownIE);
        }
    }
    private void ShieldRegen()
    {
        if (Shield < maxShield)
            Shield += Time.deltaTime * RegenShieldSpeed;
        if (Shield > maxShield) Shield = maxShield;
    }
    private IEnumerator SlowDown(float time)
    {
        speed /= 2;
        yield return new WaitForSeconds(time);
        speed *= 2;
        SLowDownIE = null;
    }
    private void DestroyObject()
    {
        Destroy(gameObject);
        moneySystem.AddMoney(money);
    }
    private void Shoot(Transform towerPostition)
    {
        GameObject bulletObject = Instantiate(bullet, transform.position+ Vector3.up*3, Aiming(towerPostition));
        Projectile projectile = bulletObject.GetComponent<Projectile>();
        projectile.IsTowerBullet = false;
        projectile.DamageHave = damage;
    }
    private Quaternion Aiming(Transform tower)
    {
        Vector3 direction = (tower.position - (transform.position+ Vector3.up * 3));
        return Quaternion.LookRotation(direction);
    }
    IEnumerator CheckingEnemy()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(1.0f / ShootSpeed);
            Collider[] Towers = Physics.OverlapSphere(transform.position, RadiusCheck, Towerlayer);
            Collider[] DefendTowers = Physics.OverlapSphere(transform.position, RadiusCheck, Defendlayer);
            if (DefendTowers.Length != 0)
            {
                Transform HighestHPTower = null;
                float HighestHp = -1;
                foreach (Collider tower in DefendTowers)
                {
                    TowerScripts towerScripts = tower.GetComponent<TowerScripts>();
                    if (towerScripts.Health + towerScripts.Shield > HighestHp)
                    {
                        HighestHp = towerScripts.Health + towerScripts.Shield;
                        HighestHPTower = tower.transform;
                    }
                }
                Shoot(HighestHPTower);
            }
            else if (Towers.Length != 0)
            {
                Transform ClosestTower = null;
                float distance = 0;
                foreach (Collider tower in Towers)
                {
                    if (Vector3.Distance(transform.position, tower.transform.position) < distance || distance == 0)
                    {
                        distance = Vector3.Distance(transform.position, tower.transform.position);
                        ClosestTower = tower.transform;
                    }
                }
                Shoot(ClosestTower);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, RadiusCheck);
    }
    public void AdJustingEnemy(float time)
    {
        maxShield *= time;
        Shield = maxShield;
        health *= time;
        damage *= time;
        money +=(int) (2*time);
    }
    IEnumerator WaitForCreatBar()
    {
        yield return new WaitForSeconds(0.2f);
        GUISystem.CreateEnemyBar(transform);
    }
}
