using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpinObjectsX : MonoBehaviour
{
    public float spinSpeed;
    private SystemManager SystemManager;
    private void Start()
    {
        SystemManager = GameObject.Find("SystemManager").GetComponent<SystemManager>();
    }
    // Update is called once per frame
    void Update()
    {
        if (SystemManager.IsGameStart == true)
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
    }
}
