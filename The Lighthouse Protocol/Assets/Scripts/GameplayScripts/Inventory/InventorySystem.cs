using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class InventorySystem : MonoBehaviour
{
    private Camera playerCamera;

    public float pickupRange = 3f;
    public Transform dropPoint;

    private Dictionary<string, int> inventory = new Dictionary<string, int>();
    private int maxInventorySize = 4;

    [System.Serializable]
    public class ItemPrefab
    {
        public string itemName;
        public GameObject prefab;
    }

    public List<ItemPrefab> itemPrefabs;
    private Dictionary<string, GameObject> prefabLookup = new Dictionary<string, GameObject>();

    public GameObject resourcePrefab;

    void Start()
    {
        playerCamera = Camera.main;

        // Populate dictionary with cleaned-up item names
        foreach (ItemPrefab item in itemPrefabs)
        {
            string cleanName = CleanItemName(item.itemName);
            prefabLookup[cleanName] = item.prefab;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickupItem();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            DropItem();
        }
    }

    void TryPickupItem()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pickupRange))
        {
            if (hit.collider.CompareTag("mats"))
            {
                string itemName = CleanItemName(hit.collider.gameObject.name);

                if (inventory.ContainsKey(itemName))
                {
                    inventory[itemName]++;
                }
                else
                {
                    if (inventory.Count >= maxInventorySize)
                    {
                        Debug.Log("Inventory full! Cannot pick up more unique items.");
                        return;
                    }
                    inventory[itemName] = 1;
                }

                Debug.Log($"Picked up {itemName}. You now have {inventory[itemName]}.");
                Destroy(hit.collider.gameObject);
            }
        }
    }

    void DropItem()
    {
        if (inventory.Count == 0) return;

        string itemToDrop = inventory.Keys.First();

        if (prefabLookup.TryGetValue(itemToDrop, out GameObject itemPrefab))
        {
            Vector3 dropPosition = playerCamera.transform.position + playerCamera.transform.forward * 1.5f;
            Instantiate(itemPrefab, dropPosition, Quaternion.identity);

            Debug.Log($"Dropped {itemToDrop}. Remaining: {inventory[itemToDrop] - 1}");
        }
        else
        {
            Debug.LogWarning($"No prefab found for {itemToDrop}. Make sure it's assigned in the Inspector.");
        }

        if (inventory[itemToDrop] > 1)
        {
            inventory[itemToDrop]--;
        }
        else
        {
            inventory.Remove(itemToDrop);
        }
    }

    public Dictionary<string, int> GetInventory()
    {
        return inventory;
    }

    // Helper function to clean Unity-generated suffixes
    private string CleanItemName(string itemName)
    {
        return itemName.Split('(')[0].Trim();
    }
}