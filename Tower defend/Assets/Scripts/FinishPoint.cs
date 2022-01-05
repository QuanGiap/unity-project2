using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FinishPoint : MonoBehaviour
{
    [SerializeField] int Health = 460;
    [SerializeField] int HealthBonuse = 40;
    [SerializeField] int HealthLost = 10;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private GUISystem gUISystem;
    private void Start()
    {
        healthText.SetText("Health: " + Health);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Health -= HealthLost;
            Destroy(other.gameObject);
            healthText.SetText("Health: " + Health);
            if (Health < 0)
            {
                gUISystem.IsGameOver = true;
                gUISystem.FreezeGame();
            }
        }
    }
    public void HealthPlus()
    {
        Health += HealthBonuse;
        healthText.SetText("Health: " + Health);
    }
}
