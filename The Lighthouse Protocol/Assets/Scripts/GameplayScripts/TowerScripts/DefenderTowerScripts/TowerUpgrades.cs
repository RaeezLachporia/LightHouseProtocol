using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgrades : MonoBehaviour
{
    private TowerShooting GatTower;
    public TowerShooting towershooting = new TowerShooting();
    public bool isUpgradable;
    public int upgradeLevel = 0;
    public float[] fireRateUpgrades = { 1f, 1.5f, 2f };
    public float[] rangeUpgrades = { 10f, 12f, 1f };
    public int[] upgradeCost = { 100, 200 };
    // Start is called before the first frame update
    void Start()
    {
        GatTower = GetComponent<TowerShooting>();
        ApplyUpgrade();
    }
    

    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpgradeTower()
    {
        if (upgradeLevel >= upgradeCost.Length)
        {
            Debug.Log("Tower is fully upgraded");
            return;
        }

        int cost = upgradeCost[upgradeLevel];

        if (GameManager.Instance.moneySpending(cost))
        {
            upgradeLevel++;
            ApplyUpgrade();
            Debug.Log("Tower upgraded to level {upgradelevel} + 1");
        }
        else
        {
            Debug.Log("Not wnough money to upgrade");
        }
    }

    private void ApplyUpgrade()
    {
        towershooting.fireRate = fireRateUpgrades[upgradeLevel];
        towershooting.twrRange = rangeUpgrades[upgradeLevel];
    }
}
