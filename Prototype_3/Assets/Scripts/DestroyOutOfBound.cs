using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBound : MonoBehaviour
{
    public float max;
    private static int a;
    private int b = 0;
    public float xPosition = -0.72f;
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < max) Destroy(gameObject);
        if (transform.position.x < xPosition && b == 0) { a += 1; Debug.Log(a);b = 1; }
    }
    private void OnCollisionEnter(Collision collision)
    {
        b = 0;
    }
}
