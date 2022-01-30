using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float MovementSpeed = 2;
    [SerializeField] private Transform Player;
    [SerializeField] private float PositionX = 0;
    [SerializeField] private float PositionY = 0;
    [SerializeField] private ParticleSystem smoke;
    private NavMeshAgent agent;
    private Rigidbody2D rigidbody;
    private Animator animator;
    bool broken;
    // Start is called before the first frame update
    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        PositionX = transform.position.x;
        PositionY = transform.position.y;
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        StartCoroutine(CheckDirection());
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (broken)
        {
            agent.isStopped = true;
            return;
        }
        agent.SetDestination(Player.position);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }
    IEnumerator CheckDirection()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            animator.SetFloat("X", (transform.position.x - PositionX)*10);
            animator.SetFloat("Y", (transform.position.y - PositionY)*10);
            PositionX = transform.position.x;
            PositionY = transform.position.y;
        }
    }
    public void Fix()
    {
        smoke.Stop();
        animator.SetTrigger("Fixed");
        broken = true;
        rigidbody.simulated = false;
    }
}
