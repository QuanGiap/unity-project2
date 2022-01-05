using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBackgorund : MonoBehaviour
{
    // Start is called before the first frame update
    public float firstx;
    public float secondx;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= secondx) transform.position = new Vector3(firstx, transform.position.y, transform.position.z);
    }
}