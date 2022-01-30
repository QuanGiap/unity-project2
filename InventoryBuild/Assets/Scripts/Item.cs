using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int ID;
    public string type;
    public string description;
    public Sprite icon;
    public bool Pickup;
    public bool equipped;
    public void Update()
    {
        if (equipped)
        {
            //Perfrome weapons act
        }
    }

    public void ItemUsage()
    {
        //weapons
        if (type == "Weapon")
        {
            equipped = true;
        }

        //health item

        //Beverage
    }
}
