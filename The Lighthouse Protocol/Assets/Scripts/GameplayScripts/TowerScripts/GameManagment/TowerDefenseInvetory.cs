using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TowerDefenseInvetory : MonoBehaviour
{
    public int invWidth = 4;
    public int invHeight = 3;
    public GameObject invSlotPrefab;
    public Transform invPanel;
    private InventorySlot[,] inventorySlots;
    private List<Item> items = new List<Item>();
    void Start()
    {
        InitializeInventory();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializeInventory()
    {
        inventorySlots = new InventorySlot[invWidth, invHeight];
        for (int y = 0; y< invHeight; y++)
        {
            for (int x = 0; x < invWidth; x++)
            {
                GameObject slotObject = Instantiate(invSlotPrefab, invPanel);
                InventorySlot slot = slotObject.GetComponent<InventorySlot>();
                inventorySlots[x, y] = slot;
            } 
        }
    }
    public bool AddingItem(Item item)
    {
        for (int y = 0; y < invHeight; y++)
        {
            for (int x = 0; x < invHeight; x++)
            {
                if (inventorySlots[x,y].isEmpty())
                {
                    inventorySlots[x, y].SetItem(item);
                    items.Add(item);
                    return true;
                }
            }
        }
        return false;
    }
    public void RemoveItem(Item item)
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.GetItem()==item)
            {
                slot.ClearSlot();
                items.Remove(item);
                break;
            }
        }
    }
}
