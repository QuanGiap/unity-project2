using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portals : MonoBehaviour
{
    public GameObject Target;
    public Vector3 OffSet;
    private Rigidbody rb;
    private void Start()
    {
        rb = GameObject.Find("Player").GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        rb.velocity = new Vector3(0, 0, 0);
        rb.angularVelocity = Vector3.zero;
        collision.transform.position = Target.transform.position + OffSet;
    }
}
