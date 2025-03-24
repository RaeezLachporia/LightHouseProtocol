using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgrades : MonoBehaviour
{
    public GameObject upgradeCanvas;
    // Start is called before the first frame update
    void Start()
    {
        
        upgradeCanvas.SetActive(false);
        
    }
    public void openUpgrade()
    {
        upgradeCanvas.SetActive(true);
    }

    public void closeUpgrade()
    {
        upgradeCanvas.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
