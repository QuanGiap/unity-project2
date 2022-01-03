using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPannel : MonoBehaviour
{
    public GameObject target;
    private Vector3 Direction;
    private PLayerController pLayerController;
    public float SpeedMultiply = 1;
    // Start is called before the first frame update
    void Start()
    {
        Direction = (target.transform.position - transform.position).normalized;
        pLayerController = GameObject.Find("Player").GetComponent<PLayerController>();
     }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        pLayerController.rb.velocity = new Vector3(0,0,0);
        pLayerController.rb.angularVelocity = Vector3.zero;
        pLayerController.rb.AddForce(Direction * pLayerController.speed*SpeedMultiply);
    }
}
