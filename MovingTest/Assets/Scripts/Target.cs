using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour
{
    public float health = 50f;
    NavMeshAgent agent;
    Transform target;
    public float distanceFace = 30;
    public int Money = 0;
    public float distanceAttack = 30f;
    public bool playerInAttackRange=false;
    public Transform Gun;
    public LayerMask WhatIsGround, WhatIsPlayer;
    public GameObject Enemyset;
    MoneySystem moneySystem;
    SpawnManager spawnManager;

    private void Start()
    {
        target = PlayerManager.instance.Player.transform;
        agent = GetComponent<NavMeshAgent>();
    }
    private void Awake()
    {
        moneySystem = GameObject.FindGameObjectWithTag("Game System").GetComponent<MoneySystem>();
        spawnManager = GameObject.FindGameObjectWithTag("Game System").GetComponent<SpawnManager>();
    }
    private void FixedUpdate()
    {
        if (target != null)
        {
            ////check range
            playerInAttackRange = Physics.CheckSphere(transform.position, distanceAttack, WhatIsPlayer);
            float distance = Vector3.Distance(target.position, transform.position);
            agent.SetDestination(target.position);
            if (distance < distanceFace) facePlayer();
        }
        else playerInAttackRange = false;
        //if (Input.GetKeyDown(KeyCode.Space)) takeDamage(20); //Remember to remove this after test
    }
    public void takeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            die();
        }
    }
    void die()
    {
        spawnManager.DropPowerUp(transform);
        Destroy(Enemyset);
        moneySystem.AddMoney(Money);
    }
    void facePlayer()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Vector3 directionGun = (target.position- Gun.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        Quaternion lookRotationGun = Quaternion.LookRotation(directionGun);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        Gun.rotation = Quaternion.Slerp(Gun.rotation, lookRotationGun, Time.deltaTime);
    }
}
