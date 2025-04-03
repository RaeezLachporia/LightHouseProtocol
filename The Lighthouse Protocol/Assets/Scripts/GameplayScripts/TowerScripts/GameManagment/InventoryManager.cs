using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventoryManager : MonoBehaviour
{

    public static Dictionary<string, int> inventoryResources = new Dictionary<string, int>();
    public Transform inventoryPanel;
    public GameObject ItemPrefab;
    public GameObject invetoryCnavas;
    private Dictionary<string, Text> resourceTexts = new Dictionary<string, Text>();
    // Start is called before the first frame update
    void Start()
    {
        invetoryCnavas.active = false;
        inventoryResources["Wood"] = 10;
        inventoryResources["Metal"] = 10;
        inventoryResources["Platic"] = 50;
        LoadCollectedResources();
        UpdateInventoryUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    void LoadCollectedResources()
    {
        Dictionary<string, int> collectedResources = CollectionPoint.GetCollectedResources();
        foreach (var item in collectedResources)
        {
            Debug.Log($"Adding {item.Key}: {item.Value} to inventory");
            if (inventoryResources.ContainsKey(item.Key))
            {
                inventoryResources[item.Key] += item.Value;
            }
            else
            {
                inventoryResources[item.Key] = item.Value;
            }
        }
        UpdateInventoryUI();
    }

    public static bool HasRequiredResources(Dictionary<string, int> requiredResources)
    {
        foreach (var item in requiredResources)
        {
            if (!inventoryResources.ContainsKey(item.Key) || inventoryResources[item.Key]<item.Value)
                return false;
                
            
        }
        return true;
    }
    public static void UseResources(Dictionary<string, int> requiredResources)
    {
        foreach (var item in requiredResources)
        {
            if (inventoryResources.ContainsKey(item.Key))
            {
                inventoryResources[item.Key] -= item.Value;
                if (inventoryResources[item.Key] <= 0)
                    inventoryResources.Remove(item.Key);
            }
        }
    }

    void UpdateInventoryUI()
    {
        
        if (inventoryPanel == null)
        {
            Debug.LogError("Inventory Panel is not assigned in the Inspector!");
            return;
        }

        // Clear existing items
        foreach (Transform child in inventoryPanel)
        {
            Destroy(child.gameObject);
        }
        resourceTexts.Clear();

        // Instantiate new UI items
        foreach (var item in inventoryResources)
        {
            Debug.Log("Updating UI for: " + item.Key + " - " + item.Value); // Log updates

            // Instantiate the new item under the inventory panel
            GameObject newItem = Instantiate(ItemPrefab, inventoryPanel);

            // Ensure it is positioned correctly in the panel
            newItem.transform.localPosition = Vector3.zero; // Reset local position if necessary

            // Get the Text component and set it
            TMP_Text resourceText = newItem.GetComponent<TMP_Text>();
            if (resourceText != null)
            {
                resourceText.text = item.Key + ": " + item.Value;
                
            }
            else
            {
                Debug.LogError("Text component missing in prefab!");
            }
        }
        /*foreach (Transform child in inventoryPanel)
        {
            Destroy(child.gameObject);
        }
        resourceTexts.Clear();
        foreach (var item in inventoryResources)
        {
            GameObject newItem = Instantiate(ItemPrefab, inventoryPanel);
            Text resourceText = newItem.GetComponent<Text>();
            if (resourceText != null)
            {
                resourceText.text = item.Key + ": " + item.Value;
                resourceTexts[item.Key] = resourceText;
            }
            else
            {
                Debug.Log("Resource item prefab missing a text component");
            }
        }*/
    }

    public void ToggleInventory()
    {
        invetoryCnavas.SetActive(!invetoryCnavas.activeSelf);
    }
   
}
