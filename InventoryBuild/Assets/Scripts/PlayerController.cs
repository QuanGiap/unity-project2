using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject Camera;
    public float speed=12;
    public float HInput=0;
    public float VInput=0;
    public CharacterController controller;
    // Start is called before the first frame update
    void Start()
    {
        rb = GameObject.Find("Player").GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        HInput = Input.GetAxis("Horizontal");
        VInput = Input.GetAxis("Vertical");
        Vector3 move = transform.right * HInput + transform.forward * VInput;
        controller.Move(move * speed*Time.deltaTime);
    }
}
