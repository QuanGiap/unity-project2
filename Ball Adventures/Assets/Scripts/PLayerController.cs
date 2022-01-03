using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayerController : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;
    public float HorizonInput;
    public float VerticalInput;
    public float speedmetter;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        HorizonInput = Input.GetAxis("Horizontal");
        VerticalInput = Input.GetAxis("Vertical");
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        if (rb.velocity.magnitude >= 0 && rb.velocity.magnitude<0.1 && ((HorizonInput != 0 && VerticalInput ==0)||(HorizonInput==0 && VerticalInput !=0)))
        {
            if (HorizonInput > 0) rb.AddForce(Vector3.right * speed);
            if (HorizonInput < 0) rb.AddForce(Vector3.left * speed);
            if (VerticalInput > 0) rb.AddForce(Vector3.forward * speed);
            if (VerticalInput < 0) rb.AddForce(Vector3.back * speed);
        }
        speedmetter = rb.velocity.magnitude;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FinishLine"))
        {
            rb.angularVelocity = Vector3.zero;
            rb.velocity = new Vector3(0, 0, 0);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Block item")|| collision.gameObject.CompareTag("Gate")) { rb.isKinematic = true;rb.isKinematic = false; };
        if (collision.gameObject.CompareTag("Enemy")) { rb.isKinematic = true; rb.isKinematic = false; Destroy(collision.gameObject,0.2f); }
    }
}
