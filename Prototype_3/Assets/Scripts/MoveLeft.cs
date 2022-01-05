using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    // Start is called before the first frame update
    public float movespeed;
    private PLayerControl playerControllerScript;
    void Start()
    {
       playerControllerScript = GameObject.Find("Player").GetComponent<PLayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
         if (playerControllerScript.gameOver == false)
             {
             transform.Translate(Vector3.left * movespeed * Time.deltaTime);
         }
    }
}
