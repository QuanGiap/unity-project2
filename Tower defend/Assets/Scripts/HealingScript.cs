using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingScript : MonoBehaviour
{
    [SerializeField] private Collider[] Towers;
    [SerializeField] private float heal = 3;
    [SerializeField] private LayerMask TowerLayer;
    public int healSpeed = 5;
    public float radius = 4f;
    private bool HealActive = true;
    // Update is called once per frame
    void Update()
    {
        if (HealActive)
        {
            Towers = Physics.OverlapSphere(transform.position, radius,TowerLayer);
            foreach (Collider tower in Towers)
            {
                TowerScripts towerScripts = tower.gameObject.GetComponent<TowerScripts>();
                if (towerScripts)
                {
                    towerScripts.IsSupport = true;
                    towerScripts.Healling(heal * healSpeed * Time.deltaTime);
                }
            }
        }
    }
    public void DisableSupport()
    {
        HealActive = false;
        foreach (Collider tower in Towers)
        {
            tower.GetComponent<TowerScripts>().IsSupport = false;
        }
    }
}
