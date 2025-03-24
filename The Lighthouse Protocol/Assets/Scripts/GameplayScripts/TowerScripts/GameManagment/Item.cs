using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Item : MonoBehaviour
{
    public string itemName;
    public Sprite icon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Item(string name, Sprite itemIcon)
    {
        itemName = name;
        icon = itemIcon;
    }
}
