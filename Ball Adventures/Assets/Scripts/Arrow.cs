using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int RotateSpeed=1;
    public float speed = 1;
    public GameObject GameOverBox;
    public float xPositionBound;
    public float xPositionSpawn;
    public float zPositionBound;
    public float zPositionBound2;
    Quaternion OppositeRotation = Quaternion.AngleAxis(90, Vector3.right);
    Quaternion OppositeRotation2 = Quaternion.AngleAxis(90, Vector3.left);
    private void FixedUpdate()
    {
        transform.Rotate(Vector3.down*RotateSpeed);
        transform.Translate(Vector3.up * speed);
        if (transform.position.x < xPositionBound) transform.position = new Vector3 (xPositionSpawn, transform.position.y, transform.position.z);
        if (transform.position.z > zPositionBound) { transform.rotation = OppositeRotation2; }
        if (transform.position.z < zPositionBound2) { transform.rotation = OppositeRotation; }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) { Destroy(other.gameObject); Destroy(gameObject); GameOverBox.gameObject.SetActive(true); }
    }
}
