using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button button;
    public Text quantText;
    private Item storedItem;
    private int itemQuantity; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ClearSlot()
    {
        storedItem = null;
        itemQuantity = 0;
        icon.enabled = false;
        quantText.text = string.Empty;
    }
    public void SetItem(Item newItem, int quantity)
    {
        storedItem = newItem;
        itemQuantity = quantity;
        icon.sprite = storedItem.icon;
        quantText.text = string.Empty;
    }

    public void onSlotClick()
    {
        Debug.Log("slot clicked " + storedItem.name);
    }
    public Item GetStoredItem()
    {
        return storedItem;
    }
    public int getItemQuantity()
    {
        return itemQuantity;
    }
    public void AddQuantity(int amount)
    {
        itemQuantity += amount;
        quantText.text = itemQuantity.ToString();
    }
    public void RemoveQuantity(int amount)
    {
        itemQuantity -= amount;
        if (itemQuantity <=0)
        {
            ClearSlot();
        }else
        {
            quantText.text = itemQuantity.ToString();
        }
    }
}
