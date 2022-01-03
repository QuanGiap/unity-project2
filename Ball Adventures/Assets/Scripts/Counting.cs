using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Counting : MonoBehaviour
{
    public int EnemyRemain;
    public TextMeshProUGUI Items;
    public Image Image;
    public bool KillAllEnemys=false;
    // Update is called once per frame
    void Update()
    {
        EnemyRemain = FindObjectsOfType<Enemy>().Length;
        if (EnemyRemain == 0) { Image.gameObject.SetActive(true); Items.gameObject.SetActive(true); KillAllEnemys = true; }
    }
}
