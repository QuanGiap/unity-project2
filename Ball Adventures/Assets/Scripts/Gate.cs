using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public Animator Animator;
    private Counting Counting;
    private void Start()
    {
        Counting = FindObjectOfType<Counting>().GetComponent<Counting>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && Counting.KillAllEnemys == true) { Animator.SetBool("GotKey", true); }
    }
}
