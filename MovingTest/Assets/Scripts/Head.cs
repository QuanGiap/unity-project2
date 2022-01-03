using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    public GameObject enemyBody;
    public int CritDamageIncrease = 2;
    public float offset = 2f;

    private void LateUpdate()
    {
        transform.position = enemyBody.transform.position + Vector3.up * offset;
    }
    public void takeHitHead(float damage)
    {
        enemyBody.GetComponent<Target>().takeDamage(damage * CritDamageIncrease);
    }
}
