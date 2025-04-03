using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgrades : MonoBehaviour
{
    [Header("Tower Upgrade Stats")]

    public int Level = 1;
    public float ProjectileDamage;
    public float DetectionRange;
    public float FireRate;
    public int TowerHealth;
    //public int upgradeCost = 50;

    [Header("References to other scripts")]
    public GameManager gamemanager = new GameManager();
    // Start is called before the first frame update
    void Start()
    {
        //gamemanager = FindObjectOfType<GameManager>();
    }



    // Update is called once per frame
    void Update()
    {

    }

    public void UpgradeTower()
    {
        Level++;
        ProjectileDamage += 5f;
        DetectionRange += 5f;
        FireRate += 3f;
    }

    /*public bool isUpgradable(int playerMoney)
    {
        playerMoney = gamemanager.currentMoney;
        if (playerMoney >= upgradeCost)
        {
            return true;
            Debug.Log("You have enough money to upgrade");
        }
        return false;
        Debug.Log("You dont have enough money to upggrade");
    }*///redundant was used for testing purposes 
}
