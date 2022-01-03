using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float SpinSpeed = 2f;
    public float MovingSpeed = 1;
    public float Height = 0.03f;
    public float duration = 10f;
    public bool IsMoney = false;
    public bool IsHealing = false;
    public bool IsShield = false;
    MoneySystem moneySystem;

    private void Start()
    {
        moneySystem = GameObject.FindGameObjectWithTag("Game System").GetComponent<MoneySystem>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
        {
            transform.Rotate(Vector3.up * SpinSpeed, Space.Self);
            transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(MovingSpeed * Time.time) * Height, transform.position.z);
        }
    }
}
