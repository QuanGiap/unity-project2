using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildCheck : MonoBehaviour
{
    private BuildSystem buildSystem;
    [SerializeField] Material material;
    [SerializeField] float ZBound = 7.6f;
    [SerializeField] float XBound = 13.64f;
    [SerializeField] float radius = 1f;
    [SerializeField] Collider[] colliders;
    // Start is called before the first frame update
    void Start()
    {
        buildSystem = GameSystemManager.Instance.buildSystem;
    }
    private void Update()
    {
        if (gameObject.transform.position.x > XBound)
        {
            gameObject.transform.position = new Vector3(XBound, 0, transform.position.z);
        }
        else if (gameObject.transform.position.x < -XBound)
        {
            gameObject.transform.position = new Vector3(-XBound, 0, transform.position.z);
        }
        if (gameObject.transform.position.z > ZBound)
        {
            gameObject.transform.position = new Vector3(transform.position.x, 0, ZBound);
        }
        else if (gameObject.transform.position.z < -ZBound)
        {
            gameObject.transform.position = new Vector3(transform.position.x, 0, -ZBound);
        }
        colliders = Physics.OverlapSphere(transform.position, radius);
        if (colliders.Length != 0)
        {
            material.color = Color.red;
            buildSystem.CanBuild = false;
        }
        else
        {
            material.color = Color.green;
            buildSystem.CanBuild = true;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
