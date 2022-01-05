using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateYAxis : MonoBehaviour
{
    [SerializeField] private float speed = -2;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }
}
