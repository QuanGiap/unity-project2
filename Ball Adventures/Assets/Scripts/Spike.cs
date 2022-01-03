using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public bool IsSpike = true;
    public float MaxTime;
    public float TimeWait;
    public float Offset;
    public GameObject GameOverBox;
    public void Start()
    {
        InvokeRepeating("TimeForSpike", TimeWait, MaxTime);
    }

    private void TimeForSpike()
    {
        IsSpike = !IsSpike;
        if (IsSpike) { transform.position = new Vector3(transform.position.x, transform.position.y + Offset, transform.position.z); }
        if (!IsSpike) { transform.position = new Vector3(transform.position.x, transform.position.y - Offset, transform.position.z); }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) { Destroy(collision.gameObject);GameOverBox.gameObject.SetActive(true); }
    }
}
