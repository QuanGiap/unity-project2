using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerScripts : MonoBehaviour , IDamageable
{
    public float RadiusCheck = 4;
    [SerializeField] private float offset = 0;
    [SerializeField] private float turnSpeed=1000;
    [SerializeField] private Transform[] ShotPosition;
    [SerializeField] private GameObject BulletType;
    [SerializeField] private LayerMask layer;
    [SerializeField] private LayerMask TowerLayer;
    public float MaxHealth = 100f;
    public float MaxShield = 100f;
    [SerializeField] private float TimeToRegen = 2;
    public bool IsRocketTower = false;
    private float shieldRegenTimer = 0;
    public float RegenShieldSpeed = 3;
    public float Damage = 10;
    public float fireRate = 3f;
    [SerializeField] private bool IsAOE = false;
    [SerializeField] private float TimeSlowDown = 1;
    public int MoneyCost = 30;
    [SerializeField] private ParticleSystem AOEParticle;
    [SerializeField] private GameObject RotateableObject;
    [TextArea(3, 10)]
    [SerializeField] private string AdditionalDescription="";
    private string DescriptionTower="";
    private bool Click = false;
    private float timeToShoot = 0;
    public float Health;
    public float Shield;
    public bool IsSupport = false;
    private int i = 0;

    private Collider[] Enemies;
    [Header("Shop Upgrade variable")]
    public float FireRateIncrease = 0.3f;
    public int FireRateCost = 10;
    public float RadiusIncrease = 0.5f;
    public int RadiusCost = 10;
    public int DamageIncrease = 5;
    public int DamageCost = 10;
    public int HealthIncrease = 20;
    public int HealthCost = 10;
    public int ShieldIncrease = 20;
    public int ShieldCost = 10;
    public int ShieldRegenSpeedIncrease = 4;
    public int ShieldRegenCost = 10;
    public int HealingRateIncrease = 3;
    public int HealingRateCost = 10;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (shieldRegenTimer < Time.time)
            ShieldRegen();
        CheckEnemyNearby();
        //Shooting system
        if (Enemies.Length != 0)
        {
            Transform ClosestEnemy = Enemies[0].transform;
            float distance = 0;
            foreach (Collider EnemyPosition in Enemies)
            {
                if (Vector3.Distance(transform.position, EnemyPosition.transform.position) < distance || distance == 0)
                {
                    distance = Vector3.Distance(transform.position, EnemyPosition.transform.position);
                    ClosestEnemy = EnemyPosition.transform;
                }
            }
            if (!IsAOE)
            { 
                FaceEnemy(ClosestEnemy);
                RaycastHit EnemyHit;
                int RandomNumber = NextPositionShoot();
                if ((Physics.Raycast(ShotPosition[RandomNumber].position, ShotPosition[RandomNumber].forward, out EnemyHit, 100, layer)) || BulletType.GetComponent<Projectile>().IsRocket)
                {
                    IsAimingAtTheEnemy(ClosestEnemy.gameObject, EnemyHit, RandomNumber);
                }
            }
            else if (Time.time >= timeToShoot)
                DealAOE(Enemies);
        }
        if (Click && Input.GetMouseButtonDown(0))
        {
            BuildSystem.DrawCircle(gameObject);
            Click = false;
        }
    }
    private void OnMouseDown()
    {
        if (!Click)
        {
            Click = true;
            SetDescription();
            BuildSystem.DrawCircle(gameObject, RadiusCheck, 0.1f);
            GameSystemManager.Instance.guiSystem.SetDescriptionText(DescriptionTower);
        }
    }
    private int NextPositionShoot()
    {
        i += 1;
        if (i == ShotPosition.Length) i = 0;
        return i;
    }
    private void FaceEnemy(Transform enemy)
    {
        Vector3 direction = (enemy.position - transform.position).normalized;
        Quaternion lookRoation = Quaternion.LookRotation(new Vector3(direction.x, RotateableObject.transform.position.y+ offset, direction.z));
        RotateableObject.transform.rotation = Quaternion.Slerp(RotateableObject.transform.rotation, lookRoation, turnSpeed*Time.deltaTime);
    }
    private void CheckEnemyNearby()
    {
        Enemies = Physics.OverlapSphere(transform.position, RadiusCheck,layer);
    }
    private void Shoot(GameObject closeEnemy,int number)
    {
        GameObject bullet = Instantiate(BulletType, ShotPosition[number].position, RotateableObject.transform.rotation);
        Projectile type = bullet.GetComponent<Projectile>();
        if (type.IsRocket)
        {
            type.AimingEnemy = closeEnemy.transform;
            type.IsFriendlyFire = IsRocketTower;
        }
        if(IsSupport) type.DamageHave = Damage*1.5f;
        else type.DamageHave = Damage;
    }
    private void IsAimingAtTheEnemy(GameObject closeEnemy, RaycastHit hit,int number)
    {
        if (Time.time >= timeToShoot && (BulletType.GetComponent<Projectile>().IsRocket || closeEnemy.GetComponent<Enemy>().ID == hit.collider.gameObject.GetComponent<Enemy>().ID))
        {
            timeToShoot = Time.time + 1f / fireRate;   //calculate next time to shot
            Shoot(closeEnemy, number);
        }
    }
    public void TakeHit(float damageTake)
    {
        shieldRegenTimer = Time.time + TimeToRegen;
        damageTake /= 2;
        if (Shield > 0)
        {
            Shield -= damageTake;
            if (Shield < 0) Shield = 0;
        }
        else
            Health -= damageTake;
        if (Health <= 0) DestroyObject();
    }
    private void DestroyObject()
    {
        HealingScript healingScript = gameObject.GetComponent<HealingScript>();
        if (healingScript) healingScript.DisableSupport();
        Destroy(gameObject);
    }
    private void DealAOE(Collider[] enemies)
    {
        AOEParticle.Play();
        float damageDeal;
        if (IsSupport) damageDeal = Damage * 1.5f;
        else damageDeal = Damage;
        foreach (Collider enemy in enemies)
        {
            IDamageable enemyType = enemy.gameObject.GetComponent<IDamageable>();
            Enemy enemySpeed = enemy.gameObject.GetComponent<Enemy>();
            if (enemyType != null) enemyType.TakeHit(damageDeal);
            if (enemySpeed) enemySpeed.SlowDownEnemy(TimeSlowDown);
        }
        timeToShoot = Time.time + 1f/fireRate;
    }
    private void ShieldRegen()
    {
        if (Shield < MaxShield)
            Shield += Time.deltaTime*RegenShieldSpeed;
        if (Shield > MaxShield) Shield = MaxShield;
    }
    public string SetDescription()
    {
        DescriptionTower = ("Damage/Shot: " + Damage + '\t' + "Health: " + Health +'\n' +
                            "Fire Rate:  " + fireRate + "\t\t" + "Shield: " + Shield +'\n');
        DescriptionTower += AdditionalDescription;
        return DescriptionTower;
    }
    public void Healling(float amount)
    {
        if (Health < MaxHealth)
            Health += amount;
    }
}
