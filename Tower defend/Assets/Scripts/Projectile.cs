using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float DamageHave = 0;
    public Transform AimingEnemy;
    [SerializeField] private float Speed;
    [SerializeField] private float ForceSpeed = 1000;
    [SerializeField] private float RadiusCheck = 1;
    [SerializeField] private LayerMask layer;
    [SerializeField] private GameObject HitEffect;
    public bool IsFriendlyFire = true;
    public bool IsRocket=false;
    public bool IsTowerBullet = true;
    [SerializeField] GameObject ExplosionEffect;
    [SerializeField] float turnSpeed = 30;
    private Rigidbody rb;
    private void Awake()
    {
        Destroy(gameObject, 4);
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        if (IsRocket)
        {
            if(AimingEnemy!= null)
                FaceEnemy(AimingEnemy);
        }
    }
    private void FaceEnemy(Transform enemy)
    {
        Vector3 direction = (enemy.position - transform.position).normalized;
        Quaternion lookRoation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRoation, turnSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PathPointer")) return;
        if (IsTowerBullet && other.gameObject.GetComponent<TowerScripts>())
        {
            return;
        }
        if (!IsTowerBullet && other.gameObject.GetComponent<Enemy>())
        {
            return;
        }
        if (other.GetComponent<Projectile>()) return;
        if (IsRocket)
        {
            Destroy(Instantiate(ExplosionEffect, transform.position, transform.rotation), 2);
            Collider[] Enemies = Physics.OverlapSphere(transform.position + Vector3.forward * 0.5f, RadiusCheck);
            foreach (Collider EnemyPosition in Enemies)
            {
                IDamageable damageable = EnemyPosition.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    if (!IsFriendlyFire && EnemyPosition.GetComponent<TowerScripts>()) continue;
                    damageable.TakeHit(DamageHave);
                }
            }
        }
        else
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null) damageable.TakeHit(DamageHave);
            Destroy(Instantiate(HitEffect, transform.position + (Vector3.forward * 0.2f), transform.rotation), 1);
        }
        Destroy(gameObject);
    }
}
