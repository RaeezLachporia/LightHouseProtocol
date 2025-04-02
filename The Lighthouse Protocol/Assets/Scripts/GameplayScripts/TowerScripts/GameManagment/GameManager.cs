using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int startingMoney = 500;
    [SerializeField] public int currentMoney;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        currentMoney = startingMoney;
        UIManager.Instance.updateMoney(currentMoney);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool moneySpending(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            UIManager.Instance.updateMoney(currentMoney);
            return true; // spent money successfully
        }
        else return false;// no money 
    }

    public void GainMoney(int amount)
    {
        currentMoney += amount;
        UIManager.Instance.updateMoney(currentMoney);
    }

    public int GetMoney()
    {
        return currentMoney;
    }

    public void AttemptUpgrade(GameObject tower)
    {
        TowerUpgrades towerUpgrades = tower.GetComponent<TowerUpgrades>();
        if (towerUpgrades != null )
        {
            if (currentMoney>= towerUpgrades.upgradeCost)
            {
                currentMoney -= towerUpgrades.upgradeCost;
                towerUpgrades.UpgradeTower();
                Debug.Log("TowerUpgraded");
            }
            else
            {
                Debug.Log("Tower Not Upgraded");
            }
        }
    }
}
