using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBound : MonoBehaviour
{
    public GameObject GameOverBox;
    public GameObject Textbox;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) { Destroy(other.gameObject); }
        GameOverBox.gameObject.SetActive(true);
        Textbox.gameObject.SetActive(true);
    }

}
