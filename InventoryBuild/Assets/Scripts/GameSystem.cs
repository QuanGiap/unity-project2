using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public GameObject Inventory;
    public bool IsInventory=false;
    public int allSlots;
    private int EnabledSlots;
    private GameObject[] slot;
    public GameObject SlotHolder;
    private void Start()
    {
        allSlots = SlotHolder.transform.childCount;
        slot = new GameObject[allSlots];
        for (int i = 0; i < allSlots; i++)
        {
            slot[i] = SlotHolder.transform.GetChild(i).gameObject;

            if (slot[i].GetComponent<Slot>().item == null)
            { slot[i].GetComponent<Slot>().empty = true; }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            IsInventory = !IsInventory;
            Inventory.gameObject.SetActive(IsInventory);            
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            GameObject itemPickUp = other.gameObject;
            Item item = itemPickUp.GetComponent<Item>();
            AddItem(itemPickUp,item.ID, item.type, item.description, item.icon);
        }
    }

    void AddItem(GameObject itemObject,int itemID,string itemType,string itemDescription, Sprite itemIcon )
    {
        for (int i = 0; i< allSlots; i++)
        { 
            if (slot[i].GetComponent<Slot>().empty)
            {
                // Add item to slot
                itemObject.GetComponent<Item>().Pickup = true;

                slot[i].GetComponent<Slot>().item = itemObject;
               slot[i].GetComponent<Slot>().icon = itemIcon;
                slot[i].GetComponent<Slot>().type = itemType;
                slot[i].GetComponent<Slot>().description = itemDescription;
                slot[i].GetComponent<Slot>().type = itemType;

                itemObject.transform.parent = slot[i].transform;
                itemObject.SetActive(false);

                slot[i].GetComponent<Slot>().UpdateSlot();
                slot[i].GetComponent<Slot>().empty = false;
                return;
            }
        }
    }
}
