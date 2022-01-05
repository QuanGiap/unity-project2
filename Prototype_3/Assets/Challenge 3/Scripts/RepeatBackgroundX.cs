using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackgroundX : MonoBehaviour
{
    private float vectorx;
    public float firstX;
    public float secondX;

    private void Start()
    {
    }

    private void Update()
    {
        vectorx = transform.position.x;
        // If background moves left by its repeat width, move it back to start position
        if (vectorx<secondX)
        {
            transform.position = new Vector3(firstX, transform.position.y, transform.position.z);
        }
    }

 
}


